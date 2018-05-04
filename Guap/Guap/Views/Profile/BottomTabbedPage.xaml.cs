using System;
using System.Threading.Tasks;
using Guap.CustomRender;
using Guap.Views.Setting;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Guap.Views.Profile
{
    using Dashboard;
    
    public partial class BottomTabbedPage : BottomTabbed
    {
        public Dashboard Dasboard { get; set; }
        public ReceivePage ReceivePage { get; set; }
        public ScanPage ScanPage { get; set; }
        public SendPage SendPage { get; set; }
        public SettingsPage SettingsPage { get; set; }

        public BottomTabbedPage()
        {
            InitializeComponent();
            
            BarTheme = BarThemeTypes.DarkWithoutAlpha;
            FixedMode = true;
            IconActiveColor = Color.FromHex("#e0bc0c");
            BarBackgroundColor = Color.Black;
                
            Dasboard = (Dashboard) (Children[0] = new Dashboard(this));
            
            Task.Run(async () => await InitBottomPages());
        }

        private async Task InitBottomPages()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            var scan = status == PermissionStatus.Granted
                ? (Page) new ScanPage(this)
                : (Page) new PermissionPage();
            
            var receive = new ReceivePage(this);
            var send = new SendPage(this);
            var setting = new SettingsPage(this);
            
            Device.BeginInvokeOnMainThread(() =>
            {
                Children.RemoveAt(1);
                Children.Insert(1, ReceivePage = receive);
                
                Children.RemoveAt(2);
                Children.Insert(2, scan);
                ScanPage = scan as ScanPage;
                
                Children.RemoveAt(3);
                Children.Insert(3, SendPage = send);
                
                Children.RemoveAt(4);
                Children.Insert(4, SettingsPage = setting);
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            Task.Run(async () =>
            {
                if (CurrentPage is PermissionPage)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                    if (status != PermissionStatus.Granted)
                    {
                        var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

                        if (result[Permission.Camera] == PermissionStatus.Granted)
                        {
                            var page = new ScanPage(this);

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Children.RemoveAt(2);
                                Children.Insert(2, ScanPage = page);

                                CurrentPage = Children[2];
                            });
                        }
                    }
                }
            });
        }
    }
}
