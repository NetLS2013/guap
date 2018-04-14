using Guap.CustomRender;

namespace Guap.Views.Profile
{
    public partial class BottomTabbedPage : BottomTabbed
    {
        public BottomTabbedPage()
        {
            InitializeComponent();
            
            BarTheme = BarThemeTypes.DarkWithoutAlpha;
            FixedMode = true;
            IconActiveColor = "#e0bc0c";
        }
    }
}