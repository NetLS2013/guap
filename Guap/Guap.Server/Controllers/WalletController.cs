using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Guap.Server.Data.Repositories;
using Guap.Server.Models;
using Guap.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace Guap.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class WalletController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notificationService;

        public WalletController(
            IUserRepository userRepository,
            INotification notificationService)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetAddressByNumber([FromBody]UserModel model)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                return BadRequest();
            }
            
            var user = await _userRepository.FindUser(model.PhoneNumber);

            return Ok(user?.Address);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateAddress([FromBody]UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return BadRequest();
            }
            
            var user = await _userRepository.FindUser(model.PhoneNumber);

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.VerificationCode.Equals(model.VerificationCode))
            {
                return BadRequest();
            }

            try
            {
                await _userRepository.UpdateAddress(model.Address, user);

                await _notificationService.Toggle(
                    new NotificationModel
                    {
                        Address = model.Address,
                        Email = user.Email,
                        NotificationsEnabled = user.NotificationsEnabled
                    });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
                
                return BadRequest();
            }

            return Ok(true);
        }
    }
}