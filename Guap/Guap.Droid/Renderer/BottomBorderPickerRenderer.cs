
using Guap.CustomRender;
using Guap.Droid.Renderer;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(BottomBorderPicker), typeof(BottomBorderPickerRenderer))]
namespace Guap.Droid.Renderer
{
    using Android.Content;
    using Android.Graphics;
    using Android.Graphics.Drawables;
    using Android.Support.V4.Content;
    using Android.Text;
    using Android.Util;
    using Android.Views;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    using Color = Android.Graphics.Color;
    using Resource = Guap.Droid.Resource;

    public class BottomBorderPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.SetBackgroundResource(Resource.Drawable.EntryBorderBottomWhite);
                Control.SetSingleLine(true);
                Control.Ellipsize = TextUtils.TruncateAt.End;

                Control.Background = AddPickerStyles("dropdown.png");
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            var layer = Context.Resources.GetDrawable(Resource.Drawable.EntryBorderBottomWhite);
            var layer1 = Context.Resources.GetDrawable(Resource.Drawable.PickerPadding);
            
            Drawable[] layers = {  layer,   GetDrawable(imagePath), layer1 };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            //layerDrawable.SetPadding(0, 0,0,0);
            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = ContextCompat.GetDrawable(this.Context, Resource.Drawable.dropdown);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;
            int height, width;
            switch (this.Context.Resources.DisplayMetrics.DensityDpi)
            {
                case DisplayMetricsDensity.Xxxhigh:
                case DisplayMetricsDensity.Xxhigh:
                    height = width = 56;
                    break;
                case DisplayMetricsDensity.D560:
                case DisplayMetricsDensity.D420:
                case DisplayMetricsDensity.D400:
                case DisplayMetricsDensity.D360:
                case DisplayMetricsDensity.Xhigh:
                    height = width = 48;
                    break;
                case DisplayMetricsDensity.High:
                default:
                    height = width = 36;
                    break;
            }

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, width, height, true));
            result.Gravity = Android.Views.GravityFlags.Right;
            
            return result;
        }
    }
}