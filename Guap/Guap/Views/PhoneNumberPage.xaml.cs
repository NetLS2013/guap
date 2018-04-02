using System;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class PhoneNumberPage : ContentPage
    {
        public PhoneNumberPage()
        {
            InitializeComponent();
        }

        private async void OpenPageNextClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhoneVerificationPage());
        }
    }
}