using System;
using Xamarin.Forms;

namespace Guap.Views
{
    using System.Collections.Generic;

    using Guap.Helpers;
    using Guap.Service;
    using Guap.ViewModels;

    using NBitcoin;

    public partial class NewUserExistPage : ContentPage
    {
        public NewUserExistPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OpenPageCreateWallet(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWalletPage());
        }

        private async void OpenPageRestoreWallet(object sender, EventArgs e)
        {
            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings() { HasNavigation = false, HeaderText = "Mnemonic Phrase" });

            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again",
                    s => EthereumService.MnenonicPhraseValidate(s)));

            inputMnemonic.ViewModel.SuccessAction = (s) =>
                {
                    // save mnenonic phrase 
                    Settings.Set(Settings.Key.MnemonicPhrase, s);

                    this.Navigation.PushAsync(
                        new SuccessSignup(
                            new CommonPageSettings()
                                {
                                    HasNavigation = false,
                                    HeaderText =
                                        "The mnenonic phrase was an exatact match."
                                        + Environment.NewLine + "Your wallet has been created."
                                        + Environment.NewLine + "Check out the dashboard."
                                },
                            () => Navigation.PushAsync(new Dashboard.Dashboard())));
                };

            await Navigation.PushAsync(inputMnemonic);
        }
    }
}