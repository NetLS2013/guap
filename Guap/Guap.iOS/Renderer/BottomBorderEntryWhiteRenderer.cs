﻿using Guap.CustomRender;
using Guap.iOS.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BottomBorderEntryWhite), typeof(BottomBorderEntryWhiteRenderer))]
namespace Guap.iOS.Renderer
{
    using CoreGraphics;

    using Guap.iOS.Helpers;

    using UIKit;

    using Xamarin.Forms.Platform.iOS;
    public class BottomBorderEntryWhiteRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                Control.Layer.BackgroundColor = UIColor.White.CGColor;
                Control.Layer.MasksToBounds = false;
                Control.Layer.ShadowColor = UICustomColor.FromHex(0x979797).CGColor;
                Control.Layer.ShadowOffset = new CGSize(0.0, 1.0f);
                Control.Layer.ShadowOpacity = 1.0f;
                Control.Layer.ShadowRadius = 0.0f;
                Control.TextColor = UIColor.Black;
            }
        }
    }
}