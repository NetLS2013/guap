using System;
using Guap.ViewModels;
using Xamarin.Forms;

namespace Guap.Views
{
    public partial class PhoneVerificationPage : ContentPage
    {
        public PhoneVerificationPage(PhoneViewModel viewModels)
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = viewModels;
        }
    }
}