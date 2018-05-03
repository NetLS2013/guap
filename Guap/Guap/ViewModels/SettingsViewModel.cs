using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Helpers;
using Guap.Models;
using Guap.Service;
using Guap.Views;
using Guap.Views.Profile;
using Guap.Views.Setting;
using Xamarin.Forms;

namespace Guap.ViewModels
{ 
    public class SettingsViewModel : BaseViewModel
    {
        private readonly Page _context;
        private readonly BottomTabbedPage _tabbedContext;
        private ObservableCollection<SettingsModel> _settingsList;

        private RequestProvider _requestProvider;

        private bool _isNotification;
        private bool _isLockApp;
        private EthereumService _ethereumService;

        public SettingsViewModel(Page context, BottomTabbedPage tabbedContext)
        {
            _context = context;
            _tabbedContext = tabbedContext;
            
            _requestProvider = new RequestProvider();
            _ethereumService = new EthereumService();
            
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
            var notifToggle = SettingsList[0].Toggled;

            try
            {
                var notification = await _requestProvider.PostAsync<UserModel, bool>(
                    GlobalSetting.Instance.NotificationsEnabledEndpoint,
                    new UserModel
                    {
                        NotificationsEnabled = notifToggle,
                        PhoneNumber = Settings.Get(Settings.Key.PhoneNumber).ToString(),
                        VerificationCode = Settings.Get(Settings.Key.VerificationCode).ToString()
                    });

                if (notification)
                {
                    Settings.Set(Settings.Key.IsNotification, notifToggle);
                }
                else
                {
                    await Task.Delay(2000).ContinueWith(_ =>
                    {
                        SettingsList[0].Toggled = !notifToggle;

                        OnPropertyChanged(nameof(SettingsList));
                    });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        });

        public ICommand LockAppCommand => new Command(async () =>
        {
            var setting = new CommonPageSettings
            {
              Title = "Unlock Wallet",
              HeaderText = "Enter your 4 digit pin",
              HasNavigation = true,
              HasBack = true
            };

            await _context.Navigation.PushSingleAsync(
                new PinAuthPage(
                    (sender, args) =>
                        {
                            this.LockApp = !LockApp;
                            Settings.Set(Settings.Key.IsLockApp, LockApp);
                            
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

        private async Task BackupWallet()
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

            await _context.Navigation.PushSingleAsync(mnemonicPage);
        }

        private async Task RestoreWallet()
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
                var result = await UpdateAddress(s);

                if (result)
                {
                    await _context.Navigation.PushSingleAsync(
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
                    
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    
                    _context.Navigation.RemovePage(
                        _context.Navigation.NavigationStack[_context.Navigation.NavigationStack.Count - 2]);

                }
            };

            await _context.Navigation.PushSingleAsync(inputMnemonic);
        }

        private async Task ResetPin()
        {
            Page page;
            
            page = new PinAuthPage(
                async (sender1, args) =>
                    {
                        var pin = args.EnteredPin;

                        await _context.Navigation.PushAsync(
                            new PinAuthPage(
                                async (sender2, args2) =>
                                    {
                                        Settings.Set(Settings.Key.Pin, pin);

                                        await _context.Navigation.PushSingleAsync(
                                            new SuccessSignup(
                                                new CommonPageSettings
                                                    {
                                                        HasNavigation = false,
                                                        HeaderText =
                                                            "Your 4 Digit pin was an exact match and been saved."
                                                    },
                                                async () => await _context.Navigation.PopAsync()));

                                        // remove pin setup pages
                                        _context.Navigation.RemovePage(_context.Navigation.NavigationStack[_context.Navigation.NavigationStack.Count - 3]);
                                        _context.Navigation.RemovePage(_context.Navigation.NavigationStack[_context.Navigation.NavigationStack.Count - 2]);
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
            
            await _context.Navigation.PushSingleAsync(page);
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

        private async Task HelpAndSupport()
        {
            await _context.Navigation.PushSingleAsync(new HelpAndSupportPage());
        }
        
        private async Task AboutGuap()
        {
            await _context.Navigation.PushSingleAsync(new AboutGuapPage());
        }

        private async Task SendFeedBack()
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

        private async Task Logout()
        {
            Settings.Set(Settings.Key.IsLogged, false);

            App.SetMainPage(new GuapPage());
        }
        
        private async Task<bool> UpdateAddress(string words)
        {
            var address = _ethereumService.GetAddress(words);
            var result = false;
            
            try
            {
                result = await _requestProvider
                    .PostAsync<UserModel, bool>(GlobalSetting.Instance.UpdateAddressEndpoint,
                        new UserModel
                        {
                            Address = address,
                            PhoneNumber = (string) Settings.Get(Settings.Key.PhoneNumber),
                            VerificationCode = (string) Settings.Get(Settings.Key.VerificationCode)
                        });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
            
            return result;
        }
    }
}