using System;
using Guap.Views.Profile;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Guap.Views.Modal
{
    public partial class ActionSelectModalPage : PopupPage
	{
	    private readonly BottomTabbedPage _tabbedContext;

		public ActionSelectModalPage(BottomTabbedPage tabbedContext)
		{
		    InitializeComponent ();
		    
		    _tabbedContext = tabbedContext;
		}

        private async void ReceiveClick(object sender, EventArgs e)
        {
            _tabbedContext.CurrentPage = _tabbedContext.Children[1];
            
            await PopupNavigation.PopAsync();
        }

        private async void SendClick(object sender, EventArgs e)
        {
            _tabbedContext.CurrentPage = _tabbedContext.Children[3];
            _tabbedContext.SendPage.SendViewModel.TokenSelectedIndex = 0;
            
            await PopupNavigation.PopAsync();
        }

	    private async void CancelClick(object sender, EventArgs e)
	    {
	        await PopupNavigation.PopAsync();
        }
	}
}