using System.Threading.Tasks;
using Guap.Server.Data.Repositories;
using Guap.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guap.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterNumber([FromBody]UserModel model)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                return BadRequest();
            }

            var user = new UserModel
            {
                PhoneNumber = model.PhoneNumber
            };
            
            await _userRepository.RegisterNumber(user);

            return Ok(true);
        }
        
        [HttpPost]
        public async Task<IActionResult> VerificationCode([FromBody]UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.VerificationCode))
            {
                return BadRequest();
            }

            var user = new UserModel
            {
                PhoneNumber = model.PhoneNumber,
                VerificationCode = model.VerificationCode
            };
            
            var result =  await _userRepository.CheckVerificationCode(user);

            if (result)
            {
                await _userRepository.CinfirmPhone(user);
            }

            return Ok(result);
        }
    }
}