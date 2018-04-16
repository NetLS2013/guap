using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guap.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Dashboard
{
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public DashboardViewModel ViewModel { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            ViewModel = new DashboardViewModel(this);
            BindingContext = ViewModel;
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            App.SetMainPage(new GuapPage());
            
            Settings.Set(Settings.Key.IsLogged, false);
        }
    }
}