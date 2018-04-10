using System;
using System.Linq;
using System.Threading.Tasks;
using Guap.Server.Data.Entities;
using Guap.Server.Models;

namespace Guap.Server.Data.Repositories
{
    public interface IUserRepository
    {
        Task RegisterNumber(UserModel model);
        Task<bool> CheckVerificationCode(UserModel model);
        Task CinfirmPhone(UserModel model);
        Task<User> FindUser(string phoneNumber);
        Task UpdateAddress(UserModel modelPhoneNumber);
    }
    
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> FindUser(string phoneNumber)
        {
            var number = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            return await Get(m => m.PhoneNumber.Contains(number));
        }

        public async Task UpdateAddress(UserModel model)
        {
            var user = await FindUser(model.PhoneNumber);

            user.Address = model.Address;

            await Update(user);
        }

        public async Task RegisterNumber(UserModel model)
        {
            var user = await FindUser(model.PhoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = model.PhoneNumber
                };
            }

            user.VerificationCode = new Random().Next(0, 999999).ToString("D6");

            await Update(user);
        }

        public async Task<bool> CheckVerificationCode(UserModel model)
        {
            var user = await FindUser(model.PhoneNumber);

            return string.Equals(user.VerificationCode, model.VerificationCode);
        }

        public async Task CinfirmPhone(UserModel model)
        {
            var user = await FindUser(model.PhoneNumber);

            user.PhoneNumberConfirmed = true;

            await Update(user);
        }
    }
}