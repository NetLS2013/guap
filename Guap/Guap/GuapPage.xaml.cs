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
            
            NavigationPage.SetHasNavigationBar(this, false);
            
            HomeContainer.Padding = new Thickness(App.ScreenHeight * 0.06, App.ScreenHeight * 0.09);
        }

        private async void OpenPageTermsClick(object sender, EventArgs e)
        {    
            await Navigation.PushAsync(new Terms());
        }
    }
}