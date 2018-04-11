using Guap.CustomRender;
using Guap.iOS.Renderer;
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
                TabBar.Translucent = false;
            }
        }
    }
}