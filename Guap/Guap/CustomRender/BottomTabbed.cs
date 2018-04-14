using Xamarin.Forms;

namespace Guap.CustomRender
{
    public class BottomTabbed : TabbedPage
    {
        public enum BarThemeTypes { Light, DarkWithAlpha, DarkWithoutAlpha }

        public bool FixedMode { get; set; }
        public BarThemeTypes BarTheme { get; set; }
        public string IconActiveColor { get; set; }

        public void RaiseCurrentPageChanged()
        {
            OnCurrentPageChanged();
        }
    }
}