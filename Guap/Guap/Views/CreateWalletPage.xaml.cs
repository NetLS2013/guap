using System;
using System.Threading.Tasks;
using Guap.Helpers;
using Guap.ViewModels;
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

            BindingContext = new CreateAccountViewModel(this);
            
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
    }
}