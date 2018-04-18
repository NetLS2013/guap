using Guap.CustomRender;
using Guap.iOS.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(FrameShadow), typeof(FrameShadowRenderer))]
namespace Guap.iOS.Renderer
{
    using System;

    using CoreGraphics;

    using UIKit;

    using Xamarin.Forms.Platform.iOS;
    public class FrameShadowRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            Layer.BorderColor = UIColor.FromRGB(207, 216, 228).CGColor;
            Layer.BorderWidth = new nfloat(1);
            Layer.CornerRadius = 2;
            
            Layer.MasksToBounds = false;
            Layer.ShadowOffset = new CGSize(1, 1.4);
            Layer.ShadowRadius = 2;
            Layer.ShadowOpacity = 0.8f;
            Layer.ShadowColor = UIColor.FromRGB(155, 155, 155).CGColor;
        }
    }
}