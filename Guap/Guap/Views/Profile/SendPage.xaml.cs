using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views.Profile
{
    public partial class SendPage : ContentPage
    {
        public SendViewModel SendViewModel { get; set; }

        public SendPage(BottomTabbedPage tabbedContext)
        {
            InitializeComponent();
            
            BindingContext =
                SendViewModel =
                    new SendViewModel(this, tabbedContext);
        }
    }
}