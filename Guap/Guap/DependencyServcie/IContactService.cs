using System.Collections.Generic;
using System.Threading.Tasks;
using Guap.Models;

namespace Guap.DependencyServcie
{
    public interface IContactService
    {
        Task<List<ContactModel>> GetContactListAsync();
    }
}