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

            BindingContext = new PhoneViewModel(this);
        }
    }
}