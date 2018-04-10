using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Guap.Server.Data.Repositories;
using Guap.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guap.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class WalletController : Controller
    {
        private readonly IUserRepository _userRepository;

        public WalletController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            try
            {
                await _userRepository.UpdateAddress(
                    new UserModel
                    {
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address
                    });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.Message}");
                
                return BadRequest();
            }

            return Ok(true);
        }
    }
}