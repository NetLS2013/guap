using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views.Setting
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            
            BindingContext = new SettingsViewModel(this);
        }
    }
}