using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Profile
{
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendPage : ContentPage
    {
        public SendViewModel SendViewModel { get; set; }

        public SendPage()
        {
            InitializeComponent();
            SendViewModel = new SendViewModel(this);
            BindingContext = SendViewModel;
        }
    }
}