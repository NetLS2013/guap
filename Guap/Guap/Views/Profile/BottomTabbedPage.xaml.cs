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
        public BottomTabbedPage()
        {
            GlobalSetting.Instance.UpdateAccountWithOutEvent();
            InitializeComponent();

            this.InitCamera();
            var dashboard = Children[0] as Dashboard;
            var scan = Children[2] as ScanPage;
            var send = Children[3] as SendPage;
            var settings = Children[4] as SettingsPage;

            send.SendViewModel.ScanEvent += () =>
                {
                    Device.BeginInvokeOnMainThread(() => CurrentPage = Children[2]);
                };

            dashboard.ViewModel.ActionSelectModalPage.Receive += () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            this.CurrentPage = this.Children[1];
                        });
                };
            dashboard.ViewModel.ActionSelectModalPage.Send += () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            this.CurrentPage = this.Children[3];
                            send.SendViewModel.TokenSelectedIndex = 0;
                        });
                };
            settings.ViewModel.LockAppSuccess += () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            this.CurrentPage = this.Children[0];
                        });
                };
            this.CurrentPageChanged += (sender, e) =>
                {
                    var currentPage = CurrentPage as PermissionPage;
                    if (currentPage != null)
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
                                        
                                        if (page != null)
                                        {
                                            page.ScanEvent += (address, amount) =>
                                                {
                                                    Device.BeginInvokeOnMainThread(() => this.CurrentPage = this.Children[3]);
                                                    send.SendViewModel.SetReceiverInfo(address, amount);
                                                };
                                        }

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
            if (scan != null)
            {
                scan.ScanEvent += (address, amount) =>
                    {
                        Device.BeginInvokeOnMainThread(() => this.CurrentPage = this.Children[3]);
                        send.SendViewModel.SetReceiverInfo(address, amount);
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