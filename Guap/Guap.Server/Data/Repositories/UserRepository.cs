using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guap.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Guap.Server.Data.Repositories
{
    public interface IUserRepository
    {
        Task RegisterNumber(string phoneNumber, User updateOrCreate);
        Task ConfirmPhone(User update);
        Task<User> FindUser(string phoneNumber);
        Task UpdateAddress(string address, User update);
        Task RegisterEmail(string email, User update);
        Task ConfirmEmail(User update);
        Task NotificationEnabled(bool notificationsEnabled, User update);
        Task<List<User>> GetAllUsers();
        Task<User> FindByEmail(string email);
    }
    
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<User> FindUser(string phoneNumber)
        {
            var number = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            return await Get(m => m.PhoneNumber.Contains(number));
        }
        
        public async Task<User> FindByEmail(string email)
        {
            return await Get(m => m.Email.Contains(email));
        }

        public async Task UpdateAddress(string address, User update)
        {
            update.Address = address;
            
            await Update(update);
        }

        public async Task RegisterEmail(string email, User update)
        {
            update.Email = email;
            update.EmailConfirmed = false;

            await Update(update);
        }

        public async Task ConfirmEmail(User update)
        {
            update.EmailConfirmed = true;

            await Update(update);
        }

        public async Task NotificationEnabled(bool notificationsEnabled, User update)
        {
            update.NotificationsEnabled = notificationsEnabled;

            await Update(update);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await GetAll().ToListAsync();
        }

        public async Task RegisterNumber(string phoneNumber, User updateOrCreate)
        {
            if (updateOrCreate == null)
            {
                updateOrCreate = new User
                {
                    PhoneNumber = phoneNumber
                };
            }

            updateOrCreate.VerificationCode = new Random().Next(0, 999999).ToString("D6");

            await Update(updateOrCreate);
        }

        public async Task ConfirmPhone(User update)
        {
            update.PhoneNumberConfirmed = true;

            await Update(update);
        }
    }
}