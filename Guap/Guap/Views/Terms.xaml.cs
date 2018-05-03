using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guap.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Terms : ContentPage
    {
        public Terms()
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OpenPageDeclineClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OpenPageAcceptClick(object sender, EventArgs e)
        {
            await Navigation.PushSingleAsync(new PhoneNumberPage());
        }
    }
}