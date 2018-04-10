using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Contacts;
using Guap.DependencyServcie;
using Guap.iOS.Service;
using Guap.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactService))]
namespace Guap.iOS.Service
{
    public class ContactService : IContactService
    {
        public Task<List<ContactModel>> GetContactListAsync()
        {
            var contactList = new List<ContactModel>();
            
            try
            {
                var keysToFetch = new[]
                {
                    CNContactKey.GivenName,
                    CNContactKey.FamilyName,
                    CNContactKey.PhoneNumbers
                };
                
                using (var store = new CNContactStore())
                {
                    var allContainers = store.GetContainers(null, out var _);
                    
                    foreach (var container in allContainers)
                    {
                        using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                        {
                            var containerResults = store.GetUnifiedContacts(predicate, keysToFetch, out _);

                            foreach (var contact in containerResults)
                            {
                                var name = string.Concat(contact.GivenName, " ", contact.FamilyName);
                                
                                for (var i = 0; i < contact.PhoneNumbers.Length; i++)
                                {
                                    var number = contact.PhoneNumbers[i];
                                    
                                    contactList.Add(new ContactModel
                                    {
                                        Name = string.Concat(name, i > 0 ? $" {i + 1}" : ""),
                                        Number = number.Value.StringValue
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.Message}");
            }

            return Task.FromResult(contactList);
        }
    }
}