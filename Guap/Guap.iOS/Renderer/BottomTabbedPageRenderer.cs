using Guap.CustomRender;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (BottomTabbed), typeof (BottomTabbedPageRenderer))]
namespace Guap.iOS.Renderer
{
    public class BottomTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged (VisualElementChangedEventArgs e)
        {
            base.OnElementChanged (e);

            if (e.NewElement != null)
            {
                UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes
                {
                    TextColor = UIColor.White
                }, UIControlState.Normal);
                
                TabBar.Translucent = false;
                TabBar.SelectedImageTintColor = ((BottomTabbed) e.NewElement).IconActiveColor.ToUIColor();
                TabBar.UnselectedItemTintColor = UIColor.White;
            }
        }
    }
}