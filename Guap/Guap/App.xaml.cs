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

            Page startPage;

            if (Equals(Settings.Get(Settings.Key.IsLogged), true) && Equals(Settings.Get(Settings.Key.IsLockApp), true))
            {
                var setting = new CommonPageSettings
                {
                    HeaderText = "Enter your 4 digit pin",
                    HasNavigation = false
                };

                var page = new BottomTabbedPage();

                startPage = new PinAuthPage(
                        (sender, args) =>
                            {
                                SetMainPage(page);
                            },
                        valid => Equals(valid, Settings.Get(Settings.Key.Pin)),
                        "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                        setting,
                        true);
            } 
            else if (Equals(Settings.Get(Settings.Key.IsLogged), true))
            {
                startPage = new BottomTabbedPage();
            }
            else if (Equals(Settings.Get(Settings.Key.ResumePage), true))
            {
                startPage = new CreateWalletPage();
            }
            else if (!Equals(Settings.Get(Settings.Key.PinSetupPage), true))
            {
                startPage = new PinAuthPage(
                    async (sender1, args) =>
                        {
                            var pin = args.EnteredPin;
                            var setting = new CommonPageSettings
                            {
                                HasNavigation = true,
                                Title = "Confirm Pin",
                                HeaderText = "Create your 4 digit pin",
                                HasBack = true
                            };
                            
                            await MainPage.Navigation.PushAsync(
                                new PinAuthPage(
                                    (sender2, args2) =>
                                        {
                                            Settings.Set(Settings.Key.Pin, pin);
                                            Settings.Set(Settings.Key.PinSetupPage, true);
                                            
                                            Settings.Set(Settings.Key.IsLockApp, false);
                                            Settings.Set(Settings.Key.IsNotification, true);

                                            SetMainPage(new GuapPage());
                                        },
                                    c => Equals(c, pin),
                                    "The 4 Digit pin you entered is incorrect.\nPlease review your pin and try again.",
                                    setting));
                        },
                    c => true,
                    string.Empty,
                    new CommonPageSettings
                    {
                        HasNavigation = false,
                        HeaderText = "Create your 4 digit pin"
                    });
            }
            else
            {
                startPage = new GuapPage();
            }
            
            SetMainPage(startPage);
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