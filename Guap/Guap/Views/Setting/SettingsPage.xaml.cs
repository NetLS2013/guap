using Guap.ViewModels;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap.Views.Setting
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel ViewModel { get; set; }
        
        public SettingsPage(BottomTabbedPage tabbedContext)
        {
            InitializeComponent();
            
            BindingContext =
                ViewModel =
                    new SettingsViewModel(this, tabbedContext);
        }
    }
}