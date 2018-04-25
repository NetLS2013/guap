using System;
using System.Collections.Generic;
using System.Diagnostics;
using Guap.Models;
using Guap.Views.Setting;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Helpers;
    using Guap.Service;
    using Guap.Views;
    using Guap.Views.Profile;

    public class SettingsViewModel : BaseViewModel
    {
        private readonly Page _context;
        private ObservableCollection<SettingsModel> _settingsList;

        private RequestProvider _requestProvider;

        private bool _isNotification;
        private bool _isLockApp;

        public event Action LockAppSuccess;

        public SettingsViewModel(Page context)
        {
            _context = context;
            _requestProvider = new RequestProvider();

            LockApp = (bool)Settings.Get(Settings.Key.IsLockApp);
            Notification = (bool)Settings.Get(Settings.Key.IsNotification);

            InitSettings();
        }

        private void InitSettings()
        {
            SettingsList = new ObservableCollection<SettingsModel>
                {
                   new SettingsModel { Title = "Notification", Icon = "notification.png", IsVisible = true,  Toggled = Notification, ToggledCommand = NotificationCommand },
                   new SettingsModel { Title = "Lock App", Icon = "notification.png", IsVisible = true,  Toggled = LockApp, ToggledCommand = LockAppCommand },
                   new SettingsModel { Title = "Restore Wallet", Method = RestoreWallet, Icon = "notification.png", },
                   new SettingsModel { Title = "Backup Wallet", Method = BackupWallet, Icon = "notification.png" },
                   new SettingsModel { Title = "Reset Pin", Method = ResetPin, Icon = "notification.png" },
                   new SettingsModel { Title = "Help & Support", Method = HelpAndSupport, Icon = "notification.png" },
                   new SettingsModel { Title = "Send Feedback", Method = SendFeedBack, Icon = "notification.png" },
                   new SettingsModel { Title = "About $Guapcoin", Method = AboutGuap, Icon = "notification.png" ,},
                   new SettingsModel { Title = "Logout", Method = Logout, Icon = "notification.png" }
                };
        }

        public ICommand NotificationCommand => new Command(async () =>
            {
                this.Notification = !this.Notification;
                try
                {
                    var notification = await _requestProvider.PostAsync<NotificationModel, string>(GlobalSetting.Instance.NotificationsEnabledEndpoint,
                        new NotificationModel
                            {
                                NotificationsEnabled = !Notification,
                                PhoneNumber = Settings.Get(Settings.Key.PhoneNumber).ToString()
                            });
                    if (!string.IsNullOrWhiteSpace(notification))
                    {
                        
                        Settings.Set(Settings.Key.IsNotification, Notification);
                    }
                    else
                    {
                        this.Notification = !Notification;

                        await Task.Delay(1000).ContinueWith(_ =>
                            {
                                SettingsList[0].Toggled = Notification;
                                OnPropertyChanged(nameof(SettingsList));
                            });
                        
                    }
                }
                catch (Exception e)
                {

                }
                
            });

        public ICommand LockAppCommand => new Command(
            () =>
                {
                    var setting = new CommonPageSettings
                                      {
                                          Title = "Unlock Wallet",
                                          HeaderText = "Enter your 4 digit pin",
                                          HasNavigation = true,
                                          HasBack = true
                                      };

                    this._context.Navigation.PushAsync(
                        new PinAuthPage(
                            (sender, args) =>
                                {
                                    this.LockApp = !this.LockApp;
                                    Settings.Set(Settings.Key.IsLockApp, LockApp);
                                    LockAppSuccess();
                                    SettingsList[1].Toggled = LockApp;
                                    this._context.Navigation.PopAsync();
                                },
                            valid => Equals(valid, Settings.Get(Settings.Key.Pin)),
                            "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                            setting));

                    SettingsList[1].Toggled = LockApp;
                });

        public bool Notification
        {
            get
            {
                return this._isNotification;
            }
            set
            {
                this._isNotification = value;
                OnPropertyChanged(nameof(Notification));
            }
        }

        public bool LockApp
        {
            get
            {
                return this._isLockApp;
            }
            set
            {
                this._isLockApp = value;
                OnPropertyChanged(nameof(LockApp));
            }
        }

        private void BackupWallet()
        {
            var mnemonicPage = new MnemonicPhrasePage(
                new CommonPageSettings
                    {
                        HasNavigation = true,
                        HasBack = true,
                        Title = "Backup Wallet",
                        HeaderText = "Mnemonic Phrase"
                    });

            mnemonicPage.viewModel.Action = () => _context.Navigation.PopAsync();
            mnemonicPage.viewModel.Words = Settings.Get(Settings.Key.MnemonicPhrase).ToString().Split(' '); 

            _context.Navigation.PushAsync(mnemonicPage);
        }

        private void RestoreWallet()
        {
            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings
                    {
                        HasNavigation = true,
                        HasBack = true,
                        HeaderText = "Restore Wallet",
                        Title = "Restore Wallet",
                        Text = "Enter your mnenonic phrase below to restore your wallet.",
                        LeftButtonText = "Cancel"
                    });

            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again.",
                    EthereumService.MnenonicPhraseValidate));

            inputMnemonic.ViewModel.SuccessAction = async s =>
                {
                    await _context.Navigation.PushAsync(
                        new SuccessSignup(
                            new CommonPageSettings
                                {
                                    HasNavigation = false,
                                    HeaderText =
                                        "The mnenonic phrase was an exatact match."
                                        + Environment.NewLine + "Your wallet has been restored."
                                        + Environment.NewLine + "Check out the dashboard."
                                },
                            () => this._context.Navigation.PopAsync()));

                    // save mnenonic phrase 
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    this._context.Navigation.RemovePage(this._context.Navigation.NavigationStack[this._context.Navigation.NavigationStack.Count - 2]);
                };

            _context.Navigation.PushAsync(inputMnemonic);
        }

        private void ResetPin()
        {
            Page page;
            page = new PinAuthPage(
                async (sender1, args) =>
                    {
                        var pin = args.EnteredPin;

                        await this._context.Navigation.PushAsync(
                            new PinAuthPage(
                                (sender2, args2) =>
                                    {
                                        Settings.Set(Settings.Key.Pin, pin);

                                        this._context.Navigation.PushAsync(
                                        new SuccessSignup(
                                            new CommonPageSettings
                                                {
                                                    HasNavigation = false,
                                                    HeaderText =
                                                        "Your 4 Digit pin was an exact match and been saved."
                                                },
                                            () => this._context.Navigation.PopAsync()));

                                        // remove pin setup pages
                                        this._context.Navigation.RemovePage(this._context.Navigation.NavigationStack[this._context.Navigation.NavigationStack.Count - 3]);
                                        this._context.Navigation.RemovePage(this._context.Navigation.NavigationStack[this._context.Navigation.NavigationStack.Count - 2]);
                                    },
                                c => Equals(c, pin),
                                "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                                new CommonPageSettings
                                    {
                                        HasNavigation = true,
                                        Title = "Reset Pin",
                                        HeaderText = "Confirm new 4 digit pin",
                                        HasBack = true
                                    }));
                    },
                c => true,
                string.Empty,
                new CommonPageSettings
                    {
                        HasNavigation = true,
                        HasBack = true,
                        Title = "Reset Pin",
                        HeaderText = "Create new 4 digit pin"
                    });
            this._context.Navigation.PushAsync(page);

        }

        public ObservableCollection<SettingsModel> SettingsList
        {
            get
            {
                return _settingsList;
            }
            set
            {
                _settingsList = value;
                
                OnPropertyChanged(nameof(SettingsList));
            }
        }

        public SettingsModel SelectedSetting
        {
            set
            {
                OnSelectedSetting(value);
                
                OnPropertyChanged(nameof(SelectedSetting));
            }
        }

        private async void OnSelectedSetting(SettingsModel settings)
        {
            if (settings == null)
                return;
            
            settings.Method?.Invoke();
        }

        private async void HelpAndSupport()
        {
            await _context.Navigation.PushAsync(new HelpAndSupportPage());
        }
        
        private async void AboutGuap()
        {
            await _context.Navigation.PushAsync(new AboutGuapPage());
        }

        private async void SendFeedBack()
        {
            var url = string.Empty;
            var appId = string.Empty;

            if (Device.RuntimePlatform == Device.iOS)
            {
                appId = "407690035";
                url = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?" +
                        $"id={appId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software";
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                appId = "com.hoteltonight.android.prod";
                url = $"market://details?id={appId}";
            }

            try
            {
                Device.OpenUri(new Uri(url));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        }

        private void Logout()
        {
            Settings.Set(Settings.Key.IsLogged, false);

            App.SetMainPage(new GuapPage());
        }
    }
}