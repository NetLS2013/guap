using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(FrameShadow), typeof(FrameShadowRenderer))]
namespace Guap.Droid.Renderer
{
    using Xamarin.Forms.Platform.Android;

    using Resource = Guap.Droid.Resource;

    public class FrameShadowRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                ViewGroup.SetBackgroundResource(Resource.Drawable.FrameShadow);
                
                Elevation = 6;
            }
        }
    }
}