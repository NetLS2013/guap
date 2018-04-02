using Android.Util;
using Guap.CustomRender;
using Guap.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LineHeightLabel), typeof(LineSpacingLabelRenderer))]
namespace Guap.Droid.Renderer
{
    public class LineSpacingLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetLineSpacing(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4.0f, Resources.DisplayMetrics), 1.0f);
            }
        }
    }
}