using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderEditor), typeof(BorderedEditorRenderer))]
namespace Guap.Droid.Renderer
{
    using Android.Graphics;
    using Android.Views;

    using Xamarin.Forms.Platform.Android;

    using Resource = Guap.Droid.Resource;

    public class BorderedEditorRenderer : EditorRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                
                Control.SetBackgroundResource(Resource.Drawable.BorderEditor);
                Control.SetLineSpacing(2, 2);
            }
        }
    }
}