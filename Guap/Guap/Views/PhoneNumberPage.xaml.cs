using System;
using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class PhoneNumberPage : ContentPage
    {
        public PhoneNumberPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new PhoneViewModel(this);
        }
    }
}