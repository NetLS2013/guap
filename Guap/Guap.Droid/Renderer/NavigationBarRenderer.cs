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

                if (e.NewElement.Children[0] is StackLayout container)
                {
                    container.HeightRequest = Resources.GetDimensionPixelSize(resourceId);
                }
            }
        }
    }
}