using System;
using System.Collections.Generic;
using System.Diagnostics;
using Guap.DependencyServcie;
using Guap.Models;
using Guap.Service;
using Guap.Views.Modal;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class ContactListViewModel : BaseViewModel
    {
        public event Action<string> SelectHandler;

        private IList<ContactModel> _contactList;
        private readonly RequestProvider _requestProvider;

        private Page _context;

        public ContactListViewModel(Page context)
        {
            _requestProvider = new RequestProvider();
            _context = context;

            Device.BeginInvokeOnMainThread(async () =>
                ContactsList = await DependencyService.Get<IContactService>().GetContactListAsync());
        }
        
        public IList<ContactModel> ContactsList
        {
            get
            {
                return _contactList;
            }
            set
            {
                _contactList = value;
                
                OnPropertyChanged(nameof(ContactsList));
            }
        }
        
        public ContactModel SelectedContact
        {
            set
            {
                OnSelectedContact(value);
                
                OnPropertyChanged(nameof(SelectedContact));
            }
        }

        private async void OnSelectedContact(ContactModel contact)
        {
            if (contact == null)
                return;
            
            try
            {
                var result = await _requestProvider
                    .PostAsync<UserModel, string>(GlobalSetting.Instance.GetAddressByNumberEndpoint, 
                        new UserModel { PhoneNumber = contact.Number });

                if (string.IsNullOrWhiteSpace(result))
                {
                    await _context.Navigation.PushPopupAsync(new InviteShareModalPage(contact));
                }
                else
                {
                    SelectHandler(result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        }
    }
}