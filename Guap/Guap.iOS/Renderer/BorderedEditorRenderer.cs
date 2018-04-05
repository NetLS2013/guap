using Guap.CustomRender;
using Guap.iOS.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BorderEditor), typeof(BorderedEditorRenderer))]
namespace Guap.iOS.Renderer
{
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class BorderedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.BorderColor = Color.FromHex("979797").ToCGColor();
                Control.Layer.BorderWidth = 2;
            }
        }
    }
}