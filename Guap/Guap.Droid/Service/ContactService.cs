using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Provider;
using Guap.DependencyServcie;
using Guap.Droid.Service;
using Guap.Models;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(ContactService))]
namespace Guap.Droid.Service
{
    public class ContactService : IContactService
    {
        public Task<List<ContactModel>> GetContactListAsync()
        {
            var contactList = new List<ContactModel>();
            
            var uri = ContactsContract.Contacts.ContentUri;
            var contentResolver = Application.Context.ContentResolver;
            
            var cursor = contentResolver.Query(uri, null, null, null, null);
            
            if (cursor != null && cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    var id = cursor.GetString(
                        cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id));
                    
                    var name = cursor.GetString(cursor.GetColumnIndex(
                        ContactsContract.Contacts.InterfaceConsts.DisplayName));

                    if (cursor.GetInt(cursor.GetColumnIndex(
                            ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber)) > 0)
                    {
                        var phoneCursor = contentResolver.Query(
                            ContactsContract.CommonDataKinds.Phone.ContentUri,
                            null,
                            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                            new [] { id },
                            null);

                        var i = 0;
                        
                        while (phoneCursor.MoveToNext())
                        {
                            contactList.Add(
                                new ContactModel
                                {
                                    Name = string.Concat(name, i > 0 ? $" {i + 1}" : ""),
                                    Number = phoneCursor.GetString(phoneCursor.GetColumnIndex(
                                        ContactsContract.CommonDataKinds.Phone.Number))
                                });

                            i++;
                        }

                        phoneCursor.Close();
                    }
                }
            }
            
            if (cursor != null)
            {
                cursor.Close();
            }

            return Task.FromResult(contactList);
        }
    }
}