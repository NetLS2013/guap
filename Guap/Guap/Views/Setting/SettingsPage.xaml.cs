using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views.Setting
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel ViewModel { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel(this);
            BindingContext = ViewModel;
        }
    }
}