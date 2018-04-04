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
            
            if ((bool) Settings.Get(Settings.Key.ResumePage))
            {
                Device.BeginInvokeOnMainThread(async () => await Navigation.PushPopupAsync(new ResumeModalPage()));
            }
        }

        private async void OpenModalResumeClick(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ResumeModalPage());
            
            Settings.Set(Settings.Key.ResumePage, true);
        }
    }
}