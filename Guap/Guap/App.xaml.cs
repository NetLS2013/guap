using Guap.Views;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(new GuapPage())
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.Black
            };

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}