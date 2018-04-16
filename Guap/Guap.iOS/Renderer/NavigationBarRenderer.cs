using Guap.CustomRender;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationBar), typeof(NavigationBarRenderer))]
namespace Guap.iOS.Renderer
{
    public class NavigationBarRenderer : VisualElementRenderer<StackLayout>
    {
        protected override void OnElementChanged (ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged (e);

            if (e.NewElement != null)
            {
                if (e.NewElement.Children[0] is StackLayout conteiner)
                {
//                    conteiner.HeightRequest =
//                        new UINavigationController().NavigationBar.Frame.Height
//                        + UIApplication.SharedApplication.StatusBarFrame.Height;
                }
            }
        }
    }
}