using Android.Views;
using Guap.CustomRender;
using Guap.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BottomBorderEntry), typeof(BottomBorderEntryRenderer))]
namespace Guap.Droid.Renderer
{
    public class BottomBorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.EntryBorderBottom);
            }
        }
    }
}