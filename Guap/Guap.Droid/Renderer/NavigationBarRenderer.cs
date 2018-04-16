using Guap.CustomRender;
using Guap.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NavigationBar), typeof(NavigationBarRenderer))]
namespace Guap.Droid.Renderer
{
    public class NavigationBarRenderer : VisualElementRenderer<StackLayout>
    {
        protected override void OnElementChanged (ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged (e);

            if (e.NewElement != null)
            {
                int resourceId = Resources.GetIdentifier ("status_bar_height", "dimen", "android");
                
                var container = e.NewElement.Children[0] as StackLayout;
                
                if (container != null)
                {
//                    container.HeightRequest = Resources.GetDimensionPixelSize(resourceId);
                }
            }
        }
    }
}