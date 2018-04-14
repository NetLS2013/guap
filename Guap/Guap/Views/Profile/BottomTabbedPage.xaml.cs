using Guap.CustomRender;

namespace Guap.Views.Profile
{
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

    using Xamarin.Forms;

    public partial class BottomTabbedPage : BottomTabbed
    {
        public BottomTabbedPage()
        {
            InitializeComponent();

            this.InitCamera();

            var scan = Children[2] as ScanPage;
            var send = Children[3] as SendPage;

            send.SendViewModel.ScanEvent += () =>
                {
                    Device.BeginInvokeOnMainThread(() => CurrentPage = Children[2]);
                };

            if (scan != null)
            {
                scan.ScanEvent += (address, amount) =>
                    {
                        send.SendViewModel.SetReceiverInfo(address, amount);
                        Device.BeginInvokeOnMainThread(() => this.CurrentPage = this.Children[3]);
                    };
            }
            

            BarTheme = BarThemeTypes.DarkWithoutAlpha;
            FixedMode = true;
            IconActiveColor = "#e0bc0c";
        }

        public async void InitCamera()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            if (status != PermissionStatus.Granted || status == PermissionStatus.Unknown)
            {
                Children[2] = new PermissionPage(Permission.Camera);
                Children[2].Title = "Scan";
            }
        }
    }
}