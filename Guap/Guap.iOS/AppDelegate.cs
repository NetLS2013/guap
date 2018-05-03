using Foundation;
using UIKit;

namespace Guap.iOS
{
    using Refractored.XamForms.PullToRefresh.iOS;

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            App.ScreenHeight = (int) UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int) UIScreen.MainScreen.Bounds.Width;

            PullToRefreshLayoutRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}