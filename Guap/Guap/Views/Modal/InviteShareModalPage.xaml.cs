using System;
using Guap.Models;
using Plugin.Messaging;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Guap.Views.Modal
{
    public partial class InviteShareModalPage : PopupPage
    {
        private readonly ContactModel _contact;

        public InviteShareModalPage(ContactModel contact)
        {
            InitializeComponent();
            
            _contact = contact;

            InviteText.FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = "We can't find " },
                    new Span { Text = _contact.Name, FontAttributes = FontAttributes.Bold, FontSize = 18 },
                    new Span { Text = " in our system.\n\nWould you like to send them a sms invitation?"}
                }
            };
        }

        private async void DismissModalClick(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void OkayModalClick(object sender, EventArgs e)
        {
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            
            if (smsMessenger.CanSendSms)
            {
                smsMessenger.SendSms(_contact.Number, "I am using Guapcoin wallet. Get it here http://guap.com:56057/invite");
            }

            await Navigation.PopPopupAsync();
        }
    }
}