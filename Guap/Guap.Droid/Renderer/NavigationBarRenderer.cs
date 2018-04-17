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
                if (e.NewElement.Children[0] is RelativeLayout container)
                {
                    container.HeightRequest = 56;
                }
            }
        }
    }
}