using System;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class NewUserExistPage : ContentPage
    {
        public NewUserExistPage()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OpenPageCreateWallet(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWallet());
        }
    }
}