using Guap.CustomRender.Pin;
using Guap.iOS.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PinItemView), typeof(PinItemViewRenderer))]
namespace Guap.iOS.Renderer
{
    using CoreGraphics;

    using UIKit;

    using Xamarin.Forms.Platform.iOS;

    public class PinItemViewRenderer : ViewRenderer<PinItemView, UIView>
    {
        private ZFRippleButton _button;

        public static new void Init()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<PinItemView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {

            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _button = new ZFRippleButton(new CGRect(0, 0, 66, 66));
                    this._button.Font = UIFont.SystemFontOfSize(Element.Text.Length > 1 ? 20 : 30);
                    _button.BackgroundColor = UIColor.Clear;
                    _button.SetTitle(Element.Text, UIControlState.Normal);
                    _button.SetTitleColor(UIColor.FromRGB(183, 186, 189), UIControlState.Normal);
                    _button.ClipsToBounds = true;
                    _button.Layer.CornerRadius = _button.Bounds.Size.Height / 2;
                    _button.Layer.BorderColor = UIColor.FromRGB(155, 155, 155).CGColor;
                    _button.Layer.BorderWidth = 2f;

                    _button.RippleColor = UIColor.FromRGB(155, 155, 155);
                    _button.RippleBackgroundColor = UIColor.Black;
                    _button.RippleOverBounds = true;
                    _button.ShadowRippleEnable = true;
                    _button.ShadowRippleRadius = 32;
                    _button.RipplePercent = 1;

                    _button.TouchUpInside += (sender, args) =>
                    {
                        Element?.Command?.Execute(Element?.CommandParameter);
                    };

                    UIView uiView = new UIView();
                    uiView.AddSubview(_button);
                    uiView.SizeToFit();

                    SetNativeControl(uiView);
                }
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _button.Center = Control.Center;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

    }
}