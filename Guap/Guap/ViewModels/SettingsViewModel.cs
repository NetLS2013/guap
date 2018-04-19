using System;
using System.Collections.Generic;
using System.Diagnostics;
using Guap.Models;
using Guap.Views.Setting;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    using Guap.Helpers;
    using Guap.Service;
    using Guap.Views;
    using Guap.Views.Profile;

    public class SettingsViewModel : BaseViewModel
    {
        private readonly Page _context;
        private List<SettingsModel> _settingsList;

        public SettingsViewModel(Page context)
        {
            _context = context;
            
            SettingsList = new List<SettingsModel>
            {
                new SettingsModel { Title = "Restore Wallet", Method = RestoreWallet },
                new SettingsModel { Title = "Backup Wallet", Method = BackupWallet },
                new SettingsModel { Title = "Reset Pin", Method = ResetPin },
                new SettingsModel { Title = "Help & Support", Method = HelpAndSupport },
                new SettingsModel { Title = "Send Feedback", Method = SendFeedBack },
                new SettingsModel { Title = "About $Guapcoin", Method = AboutGuap },
            };
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
                        Title = "Restore Wallet"
                });

            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again",
                    s => EthereumService.MnenonicPhraseValidate(s)));

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
                    GlobalSetting.Instance.UpdateAccount();
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
                                "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again",
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

        public List<SettingsModel> SettingsList
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
                if (value != null)
                {
                    OnSelectedSetting(value);
                }
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
                Debug.WriteLine($"--- Error: {e.Message}");
            }
        }
    }
}