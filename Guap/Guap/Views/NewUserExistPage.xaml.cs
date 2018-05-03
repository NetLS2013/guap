using System;
using Guap.Helpers;
using Xamarin.Forms;
using Guap.ViewModels;

namespace Guap.Views
{
    public partial class NewUserExistPage : ContentPage
    {
        public NewUserExistPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = new CreateAccountViewModel(this);
        }

        private async void OpenPageCreateAccount(object sender, EventArgs e)
        {
            await Navigation.PushSingleAsync(new CreateWalletPage());
        }
    }
}