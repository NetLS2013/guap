using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Guap.Views.Modal
{
    using Guap.Helpers;

    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;

    public partial class TransactionModelPage : PopupPage
    {
        private string _url;
        public TransactionModelPage(string title, string transactionHash)
        {
            InitializeComponent();
            this.Title.Text = title;
            _url = GlobalSetting.Instance.ExplorerTransactionEndpoint + transactionHash;
        }

        private async void DismissModalClick(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
            
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void OnNavigateExplorer(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(_url));
        }
    }
}