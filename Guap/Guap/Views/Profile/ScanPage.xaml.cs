using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Profile
{
    using System.Text.RegularExpressions;

    using Guap.Contracts;
    using Guap.ViewModels;

    using ZXing;
    using ZXing.Net.Mobile.Forms;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public event Action<string, string> ScanEvent; ZXingScannerView zxing;

        private IMessage _message;
        ZXingDefaultOverlay overlay;

        public ScanPage()
        {
            InitializeComponent();

            zxing = new ZXingScannerView
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
            zxing.IsAnalyzing = true;
            zxing.IsScanning = true;
            zxing.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            zxing.Options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
            zxing.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            zxing.AutoFocus();
            zxing.OnScanResult += OnScan;

            overlay = new ZXingDefaultOverlay
                          {
                              TopText = "Hold your phone up to the QR code",
                              BottomText = "Scanning will happen automatically",
                              ShowFlashButton = zxing.HasTorch,
                          };
            overlay.FlashButtonClicked += (sender, e) => {
                    zxing.IsTorchOn = !zxing.IsTorchOn;
                };
            var grid = new Grid
                           {
                               VerticalOptions = LayoutOptions.FillAndExpand,
                               HorizontalOptions = LayoutOptions.FillAndExpand,
                           };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            Content = grid;
            _message = DependencyService.Get<IMessage>();
        }

        private void OnScan(Result result)
        {
            try
            {
                var isValidAddress = new Regex("^(?=.{42}$)0x[a-zA-Z0-9]*").IsMatch(result.Text);
                if (isValidAddress)
                {
                    ScanEvent(result.Text, null);
                    return;
                }
                Uri uri = new Uri(result.Text);

                if (!new Regex("^(?=.{42}$)0x[a-zA-Z0-9]*").IsMatch(uri.AbsolutePath))
                {
                    throw new Exception();
                }

                var paramsQuery = ParseQueryString(uri);
                paramsQuery.TryGetValue("value", out string amount);

                ScanEvent(uri.AbsolutePath, amount);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("Scan valid QR Code."));
            }
        }

        private Dictionary<string, string> ParseQueryString(Uri uri)
        {
            var query = uri.Query.Substring(uri.Query.IndexOf('?') + 1);
            var pairs = query.Split('&');
            return pairs
                .Select(o => o.Split('='))
                .Where(items => items.Count() == 2)
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]),
                    pair => Uri.UnescapeDataString(pair[1]));
        }
    }
}