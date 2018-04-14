using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Profile
{
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionPage : ContentPage
    {
        private Permission _permission;
        public PermissionPage(Permission permission)
        {
            InitializeComponent();

            this._permission = permission;
        }

        private async void GrantPermission(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(_permission);
                if (status != PermissionStatus.Granted)
                {
                   
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}