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

    using Guap.ViewModels;

    using ZXing;
    using ZXing.Net.Mobile.Forms;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public event Action<string, string> ScanEvent; ZXingScannerView zxing;
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
               // this.scaner.
                //if (!string.IsNullOrWhiteSpace(uri.))
                //{
                    
                //}
            }
            catch (Exception e)
            {
             //   isValidAddress = false;
            }
          //  ScanEvent(result.Text);
        }
    }
}