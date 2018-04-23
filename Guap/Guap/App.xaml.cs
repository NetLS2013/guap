using System;
using Guap.CustomRender;
using Guap.Helpers;
using Guap.Views;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap
{
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

    public partial class App : Application
    {
        public static int ScreenHeight { get; set;}
        public static int ScreenWidth { get; set;}
        
        public App()
        {
            InitializeComponent();

            var startPage = typeof(GuapPage);

            if (Equals(Settings.Get(Settings.Key.IsLogged), true))
            {
                startPage = typeof(BottomTabbedPage);
            }
            else if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                startPage = typeof(CreateWalletPage);
            }
            
            if (!Equals(Settings.Get(Settings.Key.PinSetupPage), true))
            {
                var page = new PinAuthPage(
                    async (sender1, args) =>
                        {
                            var pin = args.EnteredPin;
                            
                            await MainPage.Navigation.PushAsync(
                                new PinAuthPage(
                                    (sender2, args2) =>
                                        {
                                            Settings.Set(Settings.Key.Pin, pin);
                                            Settings.Set(Settings.Key.PinSetupPage, true);

                                            SetMainPage((Page)Activator.CreateInstance(startPage));
                                        },
                                    c => Equals(c, pin),
                                    "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                                    new CommonPageSettings
                                        {
                                            HasNavigation = true,
                                            Title = "Confirm Pin",
                                            HeaderText = "Create your 4 digit pin",
                                            HasBack = true
                                        }));
                        },
                    c => true,
                    string.Empty,
                    new CommonPageSettings
                        {
                            HasNavigation = true,
                            Title = "Create Pin",
                            HeaderText = "Create your 4 digit pin"
                        });

                SetMainPage(page);
            }
            else
            {
                SetMainPage((Page)Activator.CreateInstance(startPage));
            }
        }

        public static void SetMainPage(Page page)
        {
            var navigationPage = new StyleNavigationPage(page);
            
            Current.MainPage = navigationPage;
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