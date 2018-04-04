using System;
using Guap.CustomRender;
using Guap.Helpers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Guap.Views.Modal
{
    public partial class ResumeModalPage : PopupPage
    {
        public ResumeModalPage()
        {
            InitializeComponent();
        }

        private async void DismissModalClick(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
            
            Settings.Set(Settings.Key.ResumePage, false);
        }
        
        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}