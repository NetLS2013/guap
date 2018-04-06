using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Dashboard
{
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTokenPage : ContentPage
    {
        public CreateTokenPage()
        {
            InitializeComponent();
            BindingContext = new CreateTokenViewModel(this);
        }
    }
}