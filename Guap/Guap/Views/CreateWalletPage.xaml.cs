using System;
using Guap.Helpers;
using Guap.ViewModels;
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

            BindingContext = new CreateAccountViewModel(this);
            
            if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                Device.BeginInvokeOnMainThread(async () => await Navigation.PushPopupSingleAsync(new ResumeModalPage()));
            }
        }

        private async void OpenModalResumeClick(object sender, EventArgs e)
        {
            await Navigation.PushPopupSingleAsync(new ResumeModalPage());
            
            Settings.Set(Settings.Key.ResumePage, true);
        }
    }
}