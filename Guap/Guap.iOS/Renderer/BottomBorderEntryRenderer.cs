﻿using System;
using CoreAnimation;
using CoreGraphics;
using Guap.CustomRender;
using Guap.iOS.Helpers;
using Guap.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BottomBorderEntry), typeof(BottomBorderEntryRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(BottomBorderEntryRenderer))]
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
                Control.TextAlignment = UITextAlignment.Center;
                
                Control.Layer.BackgroundColor = UIColor.Black.CGColor;
                Control.Layer.MasksToBounds = false;
                Control.Layer.ShadowColor = UICustomColor.FromHex(0x979797).CGColor;
                Control.Layer.ShadowOffset = new CGSize(0.0, 1.0f);
                Control.Layer.ShadowOpacity = 1.0f;
                Control.Layer.ShadowRadius = 0.0f;
            }
        }
    }
}