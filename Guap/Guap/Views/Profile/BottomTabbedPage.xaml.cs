using Guap.CustomRender;

namespace Guap.Views.Profile
{
    using System.Threading.Tasks;

    using Guap.Views.Dashboard;
    using Guap.Views.Setting;

    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

    using Xamarin.Forms;

    public partial class BottomTabbedPage : BottomTabbed
    {
        private SendPage _sendPage;
        
        public BottomTabbedPage()
        {
            InitializeComponent();
            
            Task.Run(async () => await InitCamera());
            Task.Run(async () => await InitBottomPages());

            CurrentPageChanged += (sender, e) =>
                {
                    if (CurrentPage is PermissionPage)
                    {
                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            var status = CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera).Result;

                            if (status != PermissionStatus.Granted)
                            {
                                var result = CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera).Result;
                                if (result.ContainsKey(Permission.Camera))
                                {
                                    status = result[Permission.Camera];
                                    if (status == PermissionStatus.Granted)
                                    {
                                        var page = new ScanPage();

                                        page.ScanEvent += (address, amount) =>
                                        {
                                            Device.BeginInvokeOnMainThread(() => this.CurrentPage = this.Children[3]);
                                            _sendPage.SendViewModel.SetReceiverInfo(address, amount);
                                        };
                                        page.ScanTokenEvent += (addressContract, addressReceiver, amount) =>
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                this.CurrentPage = this.Children[3];
                                                _sendPage.SendViewModel.SetReceiverTokenInfo(addressContract, addressReceiver, amount);
                                            });

                                        };

                                        Device.BeginInvokeOnMainThread(
                                            () =>
                                                {
                                                    Children.Insert(2, page);
                                                    Children[2].Title = "Scan";
                                                    CurrentPage = Children[2];
                                                    Children.RemoveAt(3);
                                                });
                                    }
                                }

                            }

                        }
                    }
                };

            BarTheme = BarThemeTypes.DarkWithoutAlpha;
            FixedMode = true;
            IconActiveColor = "#e0bc0c";
        }

        private async Task InitBottomPages()
        {
            var dashboard = Children[0] as Dashboard;
            var receive = (ReceivePage) (Children[1] = new ReceivePage());
            var scan = (ScanPage) (Children[2] = new ScanPage());
            _sendPage = (SendPage) (Children[3] = new SendPage());
            var settings = (SettingsPage) (Children[4] = new SettingsPage());

            dashboard.ViewModel.ActionSelectModalPage.Receive += () =>
            {
                Device.BeginInvokeOnMainThread(() => { this.CurrentPage = this.Children[1]; });
            };
            dashboard.ViewModel.ActionSelectModalPage.Send += () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.CurrentPage = this.Children[3];
                    _sendPage.SendViewModel.TokenSelectedIndex = 0;
                });
            };

            _sendPage.SendViewModel.ScanEvent += () => { Device.BeginInvokeOnMainThread(() => CurrentPage = Children[2]); };

            scan.ScanEvent += (address, amount) =>
            {
                Device.BeginInvokeOnMainThread(() => this.CurrentPage = this.Children[3]);
                _sendPage.SendViewModel.SetReceiverInfo(address, amount);
            };

            scan.ScanTokenEvent += (addressContract, addressReceiver, amount) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.CurrentPage = this.Children[3];
                    _sendPage.SendViewModel.SetReceiverTokenInfo(addressContract, addressReceiver, amount);
                });
            };

            settings.ViewModel.LockAppSuccess += () =>
            {
                Device.BeginInvokeOnMainThread(() => { this.CurrentPage = this.Children[0]; });
            };
        }

        public async Task InitCamera()
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