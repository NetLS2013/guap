using System;
using Guap.Helpers;
using Guap.Views;
using Xamarin.Forms;

namespace Guap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var startPage = typeof(GuapPage);

            if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                startPage = typeof(CreateWalletPage);
            }

            var navigationPage = new NavigationPage((Page)Activator.CreateInstance(startPage))
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