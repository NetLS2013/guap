using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Modal
{
    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;

    public partial class ActionSelectModalPage : PopupPage
	{
	    public event Action Send;

	    public event Action Receive;

		public ActionSelectModalPage()
		{
			InitializeComponent ();
		}

        private async void ReceiveClick(object sender, EventArgs e)
        {
            Receive();
            await PopupNavigation.PopAsync();
        }

        private async void SendClick(object sender, EventArgs e)
        {
            Send();
            await PopupNavigation.PopAsync();
        }
    }
}