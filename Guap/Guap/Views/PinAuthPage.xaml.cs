using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views
{
    using Guap.CustomRender.Pin;
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinAuthPage : ContentPage
    {
        private PinAuthViewModel viewModel;

        public PinAuthPage(EventHandler<PinEventArgs> successHandler, Func<IList<char>, bool> validatorFunc, string errorMessage, string headerText, bool hasNav = false, string title = "")
        {
            InitializeComponent();
            this.Title = title;
            NavigationPage.SetHasNavigationBar(this, hasNav);
            viewModel = new PinAuthViewModel();
            
            viewModel.PinViewModel.Success += successHandler;
            viewModel.PinViewModel.ValidatorFunc += validatorFunc;
            viewModel.Error = errorMessage;
            viewModel.Header = headerText;
            base.BindingContext = viewModel;
        }

        public IList<char> Pin
        {
            get
            {
                return viewModel.PinViewModel.EnteredPin;
            }
        }

        protected override bool OnBackButtonPressed() => false;
    }
}