using System;
using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views.Profile
{
    public partial class EnterAmountPage : ContentPage
    {
        public EnterAmountPage()
        {
            InitializeComponent();

            BindingContext = new EnterAmountViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InputAmount.Focus();
        }

        private async void InpitTrigger(object sender, EventArgs e)
        {
            if (!InputAmount.IsFocused)
            {
                InputAmount.Focus();
            }
        }
    }
}