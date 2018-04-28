using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Profile
{
    public partial class PermissionPage : ContentPage
    {
        public PermissionPage()
        {
            InitializeComponent();
        }

        private async void GrantPermission(object sender, EventArgs e)
        {
            CrossPermissions.Current.OpenAppSettings();   
        }
    }
}