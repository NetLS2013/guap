using Foundation;
using Guap.CustomRender;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LineHeightLabel), typeof(LineSpacingLabelRenderer))]
namespace Guap.iOS.Renderer
{
    public class LineSpacingLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var paragraphStyle = 
                    Control.AttributedText.GetAttribute("NSParagraphStyle", 0, out _) as NSMutableParagraphStyle;

                if (paragraphStyle != null)
                {
                    paragraphStyle.LineSpacing = 4.0f;
                }
                
                var attributedText = Control.AttributedText as NSMutableAttributedString;
                attributedText?.AddAttribute(new NSString("NSParagraphStyle"), paragraphStyle, new NSRange(0, Control.Text.Length));

                Control.AttributedText = attributedText;
            }
        }
    }
}