using Xamarin.Forms;

namespace Guap.Views.Profile
{
    public partial class ReceivePage : ContentPage
    {
        public ReceivePage()
        {
            InitializeComponent();

            QrResult.BarcodeOptions.Width = 500;
            QrResult.BarcodeOptions.Height = 500;
            QrResult.BarcodeValue = "123";
        }
    }
}