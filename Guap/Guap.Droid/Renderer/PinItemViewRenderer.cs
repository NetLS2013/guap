using Guap.CustomRender.Pin;
using Guap.Droid.Renderer;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;
using Android.Widget;
using System;

[assembly: ExportRenderer(typeof(PinItemView), typeof(PinItemViewRenderer))]
namespace Guap.Droid.Renderer
{
    using Android.Graphics;
    using Android.Util;

    using Guap.Droid;

    public class PinItemViewRenderer : ViewRenderer<PinItemView, AView>
    {
        private RippleButton _button;

        public static void Init()
        {
            var t = typeof(PinItemViewRenderer);
        }


        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<PinItemView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {

            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    int sideSize;

                    switch (this.Context.Resources.DisplayMetrics.DensityDpi)
                    {
                        case DisplayMetricsDensity.Xxxhigh:
                        case DisplayMetricsDensity.Xxhigh:
                        
                            sideSize = (int)ConvertDpToPixel(100);
                            break;
                        case DisplayMetricsDensity.Xhigh:
                            sideSize = (int)ConvertDpToPixel(80);
                            break;
                        case DisplayMetricsDensity.High:
                        default:
                            sideSize = (int)ConvertDpToPixel(60);
                            break;
                    }

                    _button = new RippleButton(Context);
                    _button.SetWidth(sideSize);
                    _button.SetHeight(sideSize);
                    _button.SetBackgroundResource(Resource.Drawable.PinItem);
                    _button.Text = Element.Text;
                    _button.TextSize = _button.Text.Length > 1? 20 : 30;
                    this._button.SetTextColor(Color.Rgb(183, 186, 189));
                    _button.Gravity = Android.Views.GravityFlags.Center;
                    _button.OnClick += (sender, args) =>
                    {
                        Element?.Command?.Execute(Element?.CommandParameter);
                    };

                    FrameLayout frame = new FrameLayout(Context);
                    FrameLayout.LayoutParams @params = new FrameLayout.LayoutParams(sideSize, sideSize);
                    @params.Gravity = Android.Views.GravityFlags.Center;
                    frame.AddView(_button, @params);

                    SetNativeControl(frame);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private float ConvertDpToPixel(float dp)
        {
            float density = Context.Resources.DisplayMetrics.Density;
            return (int)Math.Round((float)dp * density);
        }
    }
}