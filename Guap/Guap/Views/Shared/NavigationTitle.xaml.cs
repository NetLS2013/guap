using Guap.CustomRender;
using Guap.DependencyServcie;
using Xamarin.Forms;

namespace Guap.Views.Shared
{
    public partial class NavigationTitle : NavigationBar
    {
        public string TitleText
        {
            get { return GetValue(TitleTextProperty).ToString(); }
            set { SetValue(TitleTextProperty, value); }
        }

        private static BindableProperty TitleTextProperty = BindableProperty.Create(
            "TitleText",
            typeof(string),
            typeof(NavigationTitle),
            "",
            BindingMode.TwoWay,
            propertyChanged: TitleTextPropertyChanged);


        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (NavigationTitle) bindable;
            
            control.TextLabel.Text = newValue.ToString();
        }
        
        public NavigationTitle()
        {
            InitializeComponent();
        }    
    }
}