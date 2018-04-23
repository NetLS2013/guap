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
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailSender _emailSender;

        public AccountController(
            IUserRepository userRepository,
            ITokenProvider tokenProvider,
            IEmailSender emailSender)
        {
            _tokenProvider = tokenProvider;
            _emailSender = emailSender;
            
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
                await _userRepository.ConfirmPhone(user);
            }

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> VerificationEmail([FromBody] VerificationEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new UserModel
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                VerificationCode = model.VerificationCode
            };

            try
            {
                var result =  await _userRepository.CheckVerificationCode(user);

                if (!result)
                {
                    return BadRequest();
                }
                
                await _userRepository.RegisterEmail(user);

                var token = await _tokenProvider.GenerateAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { phone = user.PhoneNumber, token }, HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Guapcoin Support Service",
                    "Please confirm your account by clicking this link: "
                    + callbackUrl);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
                
                return BadRequest();
            }

            return Ok(true);
        }

        public async Task<IActionResult> ConfirmEmail(string phone, string token)
        {
            var user = await _userRepository.FindUser(phone);

            if (user.EmailConfirmed)
            {
                return Ok("Your account is already confirmed.");
            }

            try
            {
                await _tokenProvider.ValidateAsync(phone, token);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
                
                return BadRequest("Invalid confirmation token.");
            }
            
            return Ok("Thank you for confirming your account.");
        }
        
        public async Task<IActionResult> ForgotPin([FromBody] UserModel model)
        {
            var user = await _userRepository.FindByEmail(model.Email);

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Guapcoin Support Service",
                    "Your PIN: " + user.VerificationCode);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
            
            return Ok(true);
        }
    }
}