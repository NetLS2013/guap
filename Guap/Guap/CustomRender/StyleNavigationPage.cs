using Xamarin.Forms;

namespace Guap.CustomRender
{
    public class StyleNavigationPage : NavigationPage
    {
        public StyleNavigationPage (Page root)
            : base(root)
        {
            BarTextColor = Color.White;
            BarBackgroundColor = Color.Black;
        }
    }
}