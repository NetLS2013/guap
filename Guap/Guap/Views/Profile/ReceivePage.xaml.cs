using Xamarin.Forms;

namespace Guap.Views.Profile
{
    public partial class ReceivePage : ContentPage
    {
        public ReceivePage()
        {
            InitializeComponent();

            var qrHeight = (int)(App.ScreenHeight * 0.45);
            qrHeight = qrHeight < 160 ? 300 : qrHeight;
            
            QrWrapper.HeightRequest = qrHeight;
            
            QrResult.BarcodeOptions.Width = qrHeight;
            QrResult.BarcodeOptions.Height = qrHeight;
            QrResult.BarcodeValue = "123";
        }
    }
}