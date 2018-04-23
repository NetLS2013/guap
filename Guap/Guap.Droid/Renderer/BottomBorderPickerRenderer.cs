
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
                Control.SetBackgroundResource(Resource.Drawable.EntryBorderBottomWhite);
                Control.SetSingleLine(true);
                Control.Ellipsize = TextUtils.TruncateAt.End;

                Control.Background = AddPickerStyles("dropdown.png");
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            Drawable[] layers = { Context.Resources.GetDrawable(Resource.Drawable.EntryBorderBottomWhite), GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            
            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = ContextCompat.GetDrawable(this.Context, Resource.Drawable.dropdown);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 56, 56, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }
    }
}