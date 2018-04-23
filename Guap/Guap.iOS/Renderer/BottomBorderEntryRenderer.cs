using CoreGraphics;
using Guap.CustomRender;
using Guap.iOS.Helpers;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BottomBorderEntry), typeof(BottomBorderEntryRenderer))]
namespace Guap.iOS.Renderer
{
    public class BottomBorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                
                Control.Layer.BackgroundColor = UIColor.Black.CGColor;
                Control.Layer.MasksToBounds = false;
                Control.Layer.ShadowColor = UIColor.White.CGColor;
                Control.Layer.ShadowOffset = new CGSize(0.0, 1.0f);
                Control.Layer.ShadowOpacity = 1.0f;
                Control.Layer.ShadowRadius = 0.0f;
                Control.TextColor = UIColor.White;
            }
        }
    }
}