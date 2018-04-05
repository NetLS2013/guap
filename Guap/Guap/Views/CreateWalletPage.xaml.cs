using System;
using System.Threading.Tasks;
using Guap.Helpers;
using Guap.Views.Modal;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Guap.Views
{
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
            await Navigation.PushAsync(new MnemonicPhrasePage(new CommonPageSettings(){HasNavigation = false, HeaderText = "Mnemonic Phrase"}));
        }
    }
}