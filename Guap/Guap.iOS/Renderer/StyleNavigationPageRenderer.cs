using Guap.CustomRender;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StyleNavigationPage), typeof(StyleNavigationPageRenderer))]
namespace Guap.iOS.Renderer
{
    public class StyleNavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            
            if (e.NewElement != null)
            {
                var att = new UITextAttributes();

                att.Font = UIFont.BoldSystemFontOfSize(20);
                
                UINavigationBar.Appearance.SetTitleTextAttributes(att);
            }
        }
    }
}