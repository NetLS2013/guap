using System;
using Xamarin.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Net.Mobile.Forms;

namespace Guap.Views.Profile
{
    using Guap.ViewModels;

    public partial class ReceivePage : ContentPage
    {
        public ReceiveViewModel ViewModel;
        
        public ReceivePage(BottomTabbedPage tabbedContext)
        {
            InitializeComponent();
            
            BindingContext =
                ViewModel =
                    new ReceiveViewModel(this, tabbedContext);

            var qrHeight = (int) (App.ScreenHeight * 0.45);
            
            Device.BeginInvokeOnMainThread(() =>
            {
                var qrImageView = new ZXingBarcodeImageView
                {
                    BarcodeFormat = BarcodeFormat.QR_CODE,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BarcodeOptions = new EncodingOptions
                    {
                        Width = qrHeight,
                        Height = qrHeight
                    }
                };
                
                qrImageView.SetBinding(
                    ZXingBarcodeImageView.BarcodeValueProperty,
                    new Binding(nameof(ViewModel.RequestString)));
                
                QrWrapper.HeightRequest = qrHeight;
                QrWrapper.Children.Add(qrImageView);
            });
        }
    }
}