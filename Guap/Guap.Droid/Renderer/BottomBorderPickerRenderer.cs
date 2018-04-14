
using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BottomBorderPicker), typeof(BottomBorderPickerRenderer))]
namespace Guap.Droid.Renderer
{
    using Android.Text;
    using Android.Views;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using Color = Android.Graphics.Color;
    using Resource = Guap.Droid.Resource;

    public class BottomBorderPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.EntryBorderBottomWhite);
                Control.SetSingleLine(true);
                Control.Ellipsize = TextUtils.TruncateAt.End;
            }
        }
    }
}