using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views
{
    using Guap.Helpers;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessSignup : ContentPage
    {
        private Action nextAction;

        public SuccessSignup(CommonPageSettings pageSettings, Action nextAction)
        {
            InitializeComponent();

            // init top bar
            NavigationPage.SetHasNavigationBar(this, pageSettings.HasNavigation);
            NavigationPage.SetHasBackButton(this, pageSettings.HasBack);
            this.Title = pageSettings.Title;

            this.nextAction = nextAction;

            Message.Text = pageSettings.HeaderText;
        }

        private async void OpenPageNewUserClick(object sender, EventArgs e)
        {
            this.nextAction();
        }
    }
}