using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class ForgotPinPage : ContentPage
    {
        public ForgotPinPage()
        {
            InitializeComponent();

            BindingContext = new CreateAccountViewModel(this);
        }
    }
}