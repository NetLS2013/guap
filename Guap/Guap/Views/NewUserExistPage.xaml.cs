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
            
            BindingContext = new CreateAccountViewModel(this);
        }

        private async void OpenPageCreateWallet(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWalletPage());
        }
    }
}