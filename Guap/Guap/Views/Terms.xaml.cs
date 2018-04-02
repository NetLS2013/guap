using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private async void OpenPageDeclineClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OpenPageAcceptClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhoneNumberPage());
        }
    }
}