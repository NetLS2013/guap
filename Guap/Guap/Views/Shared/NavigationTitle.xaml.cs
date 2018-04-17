using System;
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
        
        public double TitleFontSize
        {
            get { return Convert.ToDouble(GetValue(TitleFontSizeProperty)); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        
        public bool HasIcon
        {
            get { return Convert.ToBoolean(GetValue(HasIconProperty)); }
            set { SetValue(HasIconProperty, value); }
        }

        private static BindableProperty TitleTextProperty = BindableProperty.Create(
            "TitleText",
            typeof(string),
            typeof(NavigationTitle),
            "",
            BindingMode.TwoWay,
            propertyChanged: TitleTextPropertyChanged);


        private static BindableProperty TitleFontSizeProperty = BindableProperty.Create(
            "TitleFontSize",
            typeof(double),
            typeof(NavigationTitle),
            (double)18,
            BindingMode.TwoWay,
            propertyChanged: TitleFontSizePropertyChanged);
        
        private static BindableProperty HasIconProperty = BindableProperty.Create(
            "TitleFontSize",
            typeof(bool),
            typeof(NavigationTitle),
            false,
            BindingMode.TwoWay,
            propertyChanged: HasIconPropertyChanged);
        
        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (NavigationTitle) bindable;
            
            control.TextLabel.Text = newValue.ToString();
        }
        
        private static void TitleFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (NavigationTitle) bindable;

            control.TextLabel.FontSize = Convert.ToDouble(newValue);
        }
        
        private static void HasIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (NavigationTitle) bindable;

            control.IconImage.IsVisible = Convert.ToBoolean(newValue);
        }
        
        public NavigationTitle()
        {
            InitializeComponent();
        }    
    }
}