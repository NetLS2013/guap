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
        public Dashboard()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel(this);
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            App.SetMainPage(new GuapPage());
            
            Settings.Set(Settings.Key.IsLogged, false);
        }
    }
}