using System;
using System.IO;
using System.Threading.Tasks;
using Guap.Server.Data.Repositories;
using Guap.Server.Models;
using Newtonsoft.Json;

namespace Guap.Server.Service
{
    public interface ITokenProvider
    {
        Task<string> GenerateAsync(string phoneNumber);
        Task<bool> ValidateAsync(string phoneNumber, string token);
    }
    
    public class TokenProvider : ITokenProvider
    {
        private readonly IUserRepository _userRepository;

        public TokenProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<string> GenerateAsync(string phoneNumber)
        {
            var ms = new MemoryStream();

            using (var streamWriter = new StreamWriter(ms))
            {
                using (var writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartObject();
                    {
                        writer.WritePropertyName("DateTimeOffset");
                        writer.WriteValue(DateTimeOffset.UtcNow);
                        writer.WritePropertyName("PhoneNumber");
                        writer.WriteValue(phoneNumber);
                    }
                    writer.WriteEndObject();
                }
            }

            return Convert.ToBase64String(ms.ToArray());
        }
        
        public async Task<bool> ValidateAsync(string phoneNumber, string token)
        {
            var ms = new MemoryStream(Convert.FromBase64String(token));
            
            using (var reader = new StreamReader(ms))
            {
                var deserializeToken = JsonConvert.DeserializeObject<TokenModel>(reader.ReadToEnd(), 
                    new JsonSerializerSettings { Formatting = Formatting.Indented });
                
                var expirationTime = deserializeToken.DateTimeOffset + TimeSpan.FromMinutes(5);
                
                if (expirationTime < DateTimeOffset.UtcNow)
                {
                    throw new Exception("Token time expired.");
                }

                var user = await _userRepository.FindUser(phoneNumber);

                if (user == null || user.PhoneNumber != deserializeToken.PhoneNumber)
                {
                    throw new Exception("Wrong phone number.");
                }
                
                await _userRepository.ConfirmEmail(user);
            }

            return true;
        }
    }
}