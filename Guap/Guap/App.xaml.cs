using System;
using Guap.Helpers;
using Guap.Views;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set;}
        public static int ScreenWidth { get; set;}
        
        public App()
        {
            InitializeComponent();

            var startPage = typeof(GuapPage);

            if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                startPage = typeof(CreateWalletPage);
            }
            if (!Equals(Settings.Get(Settings.Key.PinSetupPage), true))
            {
                var page = new PinAuthPage(
                    (sender1, args) =>
                        {
                            var pin = args.EnteredPin;
                            MainPage.Navigation.PushAsync(
                                new PinAuthPage(
                                    (sender2, args2) =>
                                        {
                                            Settings.Set(Settings.Key.Pin, pin);
                                            Settings.Set(Settings.Key.PinSetupPage, true);

                                            var navigationPage =
                                                new NavigationPage((Page)Activator.CreateInstance(startPage))
                                                    {
                                                        BarTextColor
                                                            = Color
                                                                .White,
                                                        BarBackgroundColor
                                                            = Color
                                                                .Black
                                                    };
                                            MainPage = navigationPage;
                                        },
                                    c => Equals(c, pin),
                                    "The 4 Digit pin you entered is incorrect." + Environment.NewLine
                                    + "Please review your pin and try again",
                                    new CommonPageSettings()
                                        {
                                            HasNavigation = true,
                                            Title = "Confirm Pin",
                                            HeaderText = "Create your 4 digit pin",
                                            HasBack = true
                                        }));
                        },
                    c => true,
                    string.Empty,
                    new CommonPageSettings()
                        {
                            HasNavigation = true,
                            Title = "Create Pin",
                            HeaderText = "Create your 4 digit pin"
                        });

                MainPage = new NavigationPage(page) { BarTextColor = Color.White, BarBackgroundColor = Color.Black };
            }
            else
            {
                var navigationPage =
                    new NavigationPage((Page)Activator.CreateInstance(startPage))
                        {
                            BarTextColor = Color.White,
                            BarBackgroundColor = Color.Black
                        };
                MainPage = navigationPage;
            }
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