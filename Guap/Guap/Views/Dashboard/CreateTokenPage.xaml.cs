using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Dashboard
{
    using Guap.Models;
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTokenPage : ContentPage
    {
        public CreateTokenPage(DashboardViewModel viewModel, Token token = null)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            BindingContext = new CreateTokenViewModel(this, viewModel, token);
        }
    }
}