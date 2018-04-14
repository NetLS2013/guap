using System;
using Guap.CustomRender.Pin;
using Guap.Helpers;
using Guap.Views;
using Guap.Views.Dashboard;
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

            if (Settings.Get(Settings.Key.IsLogged) != null)
            {
                LoginButtonWrapper.IsVisible = true;
            }
        }

        private async void OpenPageTermsClick(object sender, EventArgs e)
        {    
            await Navigation.PushAsync(new Terms());
        }

        private async void OpenPageLoginClick(object sender, EventArgs e)
        {
            var succesHandler = new EventHandler<PinEventArgs>((pinSender, pinEventArgs) =>
            {
                App.SetMainPage(new Dashboard());
                
                Settings.Set(Settings.Key.IsLogged, true);
            });

            var setting = new CommonPageSettings
            {
                Title = "Unlock Wallet",
                HeaderText = "Enter your 4 digit pin"
            };
                
            await Navigation.PushAsync(
                new PinAuthPage(
                    succesHandler,
                    valid => Equals(valid, Settings.Get(Settings.Key.Pin)),
                    "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                    setting));
        }
    }
}