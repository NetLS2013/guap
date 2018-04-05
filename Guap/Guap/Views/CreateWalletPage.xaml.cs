using System;
using System.Threading.Tasks;
using Guap.Helpers;
using Guap.Views.Modal;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Guap.Views
{
    using System.Collections.Generic;

    using Guap.Service;

    public partial class CreateWalletPage : ContentPage
    {
        public CreateWalletPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            
            if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                Device.BeginInvokeOnMainThread(async () => await Navigation.PushPopupAsync(new ResumeModalPage()));
            }
        }

        private async void OpenModalResumeClick(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ResumeModalPage());
            
            Settings.Set(Settings.Key.ResumePage, true);
        }

        private async void OpenMnemonicClick(object sender, EventArgs e)
        {
            var words = EthereumService.MnenonicPhraseGenerate();

            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings() { HasNavigation = false, HeaderText = "Mnemonic Phrase" });
            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again",
                    s => string.Join(" ", words) == s));


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

            var mnemonicPage = new MnemonicPhrasePage(
                new CommonPageSettings() { HasNavigation = false, HeaderText = "Mnemonic Phrase" });
            mnemonicPage.viewModel.Action = () => Navigation.PushAsync(inputMnemonic);
            mnemonicPage.viewModel.Words = words;

            await Navigation.PushAsync(mnemonicPage);

        }
    }
}