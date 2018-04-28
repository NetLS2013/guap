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

    using Nethereum.Hex.HexConvertors;
    using Nethereum.Hex.HexConvertors.Extensions;
    using Nethereum.Hex.HexTypes;

    using ZXing;
    using ZXing.Net.Mobile.Forms;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly BottomTabbedPage _tabbedContext;
        ZXingScannerView zxing;

        private IMessage _message;
        ZXingDefaultOverlay overlay;

        public ScanPage(BottomTabbedPage tabbedContext)
        {
            InitializeComponent();

            _tabbedContext = tabbedContext;
            
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
                if (paramsQuery.TryGetValue("value", out string amount))
                {
                    ScanEvent(uri.AbsolutePath, amount);
                    return;
                }

                if (paramsQuery.TryGetValue("function", out string function))
                {
                    if (function.Contains("transfer"))
                    {
                        var address = new Regex("(?<=address ).{42}").Match(result.Text).Value;
                        var amountToken = new Regex("(?<=uint )[0-9]*").Match(result.Text).Value;
                        
                        ScanEvent(uri.AbsolutePath, address, amountToken);
                        
                        return;
                    }
                }

                if (paramsQuery.TryGetValue("data", out string data))
                {
                    if (data.StartsWith("0xa9059cbb"))
                    {
                        data = data.Replace("0xa9059cbb", string.Empty);
                        
                        var address = new HexBigInteger(data.Substring(data.Length - 104, 40)).HexValue;
                        var amountToken = new HexBigInteger(data.Substring(data.Length - 64)).Value.ToString();
                        
                        ScanEvent(uri.AbsolutePath, address, amountToken);
                        
                        return;
                    }
                }

                ScanEvent(uri.AbsolutePath, null);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("Scan valid QR Code."));
            }
        }

        void ScanEvent(string address, string amount)
        {
            _tabbedContext.CurrentPage = _tabbedContext.Children[3];
            _tabbedContext.SendPage.SendViewModel.SetReceiverInfo(address, amount);
        }

        void ScanEvent(string addressContract, string addressReceiver, string amount)
        {
            _tabbedContext.CurrentPage = _tabbedContext.Children[3];
            _tabbedContext.SendPage.SendViewModel.SetReceiverTokenInfo(addressContract, addressReceiver, amount);
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