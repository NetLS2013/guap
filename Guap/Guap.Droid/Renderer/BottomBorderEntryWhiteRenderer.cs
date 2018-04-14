using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BottomBorderEntryWhite), typeof(BottomBorderEntryWhiteRenderer))]
namespace Guap.Droid.Renderer
{
    using Android.Text.Method;

    using Java.Util;

    using Xamarin.Forms.Platform.Android;

    using Resource = Guap.Droid.Resource;

    public class BottomBorderEntryWhiteRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.EntryBorderBottomWhite);
            }
        }
    }
}