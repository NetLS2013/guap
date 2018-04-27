using Android.Util;
using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderEditor), typeof(BorderedEditorRenderer))]
namespace Guap.Droid.Renderer
{
    public class BorderedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            
            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.BorderEditor);
                Control.SetLineSpacing(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4.0f, Resources.DisplayMetrics), 1.0f);
            }
        }
    }
}