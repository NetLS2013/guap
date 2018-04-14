using System;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class EnterEmailPage : ContentPage
    {
        public EnterEmailPage()
        {
            InitializeComponent();
        }

        private async void OpenPageCreateWallet(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWalletPage());
        }
    }
}