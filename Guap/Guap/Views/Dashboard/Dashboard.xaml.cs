using Guap.ViewModels;
using Guap.Views.Profile;
using Xamarin.Forms;

namespace Guap.Views.Dashboard
{
    public partial class Dashboard : ContentPage
    {
        public DashboardViewModel ViewModel { get; set; }

        public Dashboard(BottomTabbedPage tabbedContext)
        {
            InitializeComponent();
            
            BindingContext =
                ViewModel =
                    new DashboardViewModel(this, tabbedContext);
        }
    }
}