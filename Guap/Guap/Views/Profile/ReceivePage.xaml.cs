using System;
using Xamarin.Forms;

namespace Guap.Views.Profile
{
    using Guap.ViewModels;

    public partial class ReceivePage : ContentPage
    {
        public ReceivePage()
        {
            InitializeComponent();
            BindingContext = new ReceiveViewModel(this);

            var qrHeight = (int) (App.ScreenHeight * 0.45);
            
            QrWrapper.HeightRequest = qrHeight;
            
            QrResult.BarcodeOptions.Width = qrHeight;
            QrResult.BarcodeOptions.Height = qrHeight;
        }
    }
}