using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class EnterEmailPage : ContentPage
    {
        public EnterEmailPage()
        {
            InitializeComponent();

            BindingContext = new CreateAccountViewModel(this);
        }
    }
}