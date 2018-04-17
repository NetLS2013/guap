using Android.Graphics;
using Guap.CustomRender;
using Guap.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(StyleNavigationPage), typeof(StyleNavigationPageRenderer))]
namespace Guap.Droid.Renderer
{
    public class StyleNavigationPageRenderer : NavigationPageRenderer
    {
        private Android.Support.V7.Widget.Toolbar _toolbar;

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);
            
            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                _toolbar = (Android.Support.V7.Widget.Toolbar) child;
                _toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
            }
        }

        private void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            if(e.Child.GetType() == typeof(Android.Support.V7.Widget.AppCompatTextView))
            {
                var textView = (Android.Widget.TextView) e.Child;
//                var spaceFont = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "space_age.ttf");
//                
//                textView.Typeface = spaceFont;
                textView.TextSize = 20;
                textView.SetTypeface(null, TypefaceStyle.Bold);
                
                _toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }
        }
    }
}