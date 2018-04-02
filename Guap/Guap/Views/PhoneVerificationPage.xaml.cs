using System;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class PhoneVerificationPage : ContentPage
    {
        public PhoneVerificationPage()
        {
            InitializeComponent();
        }

        private async void OpenPageNextClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SuccessSignup());
        }
    }
}