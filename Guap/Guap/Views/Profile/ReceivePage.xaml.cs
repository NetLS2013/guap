using System;
using Xamarin.Forms;

namespace Guap.Views.Profile
{
    public partial class ReceivePage : ContentPage
    {
        public ReceivePage()
        {
            InitializeComponent();

            var qrHeight = (int) (App.ScreenHeight * 0.45);
            
            QrWrapper.HeightRequest = qrHeight;
            
            QrResult.BarcodeOptions.Width = qrHeight;
            QrResult.BarcodeOptions.Height = qrHeight;
            QrResult.BarcodeValue = "123";
        }

        private async void OpenPageContactsClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactListPage());
        }
    }
}