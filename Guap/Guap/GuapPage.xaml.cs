using System;
using Guap.Views;
using Xamarin.Forms;

namespace Guap
{
    public partial class GuapPage : ContentPage
    {
        public GuapPage()
        {
            InitializeComponent();
        }

        private async void OpenPageTermsClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Terms());
        }
    }
}