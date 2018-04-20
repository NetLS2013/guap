using System;
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
            await Navigation.PushAsync(new CreateWalletPage());
        }
    }
}