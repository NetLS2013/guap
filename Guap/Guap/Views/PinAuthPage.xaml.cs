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
    using Guap.Helpers;
    using Guap.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinAuthPage : ContentPage
    {
        private PinAuthViewModel viewModel;

        public PinAuthPage(EventHandler<PinEventArgs> successHandler, Func<string, bool> validatorFunc, string errorMessage, CommonPageSettings pageSettings, bool isReset = false)
        {
            this.Title = pageSettings.Title;
            
            NavigationPage.SetHasNavigationBar(this, pageSettings.HasNavigation);
            NavigationPage.SetHasBackButton(this, pageSettings.HasBack);
            InitializeComponent();
            
            viewModel = new PinAuthViewModel(pageSettings, isReset);
            
            viewModel.PinViewModel.Success += successHandler;
            viewModel.PinViewModel.ValidatorFunc += validatorFunc;
            viewModel.Error = errorMessage;
            viewModel.Header = pageSettings.HeaderText;
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