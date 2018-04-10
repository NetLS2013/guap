using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Models
{
    public class UserModel
    {
        [Phone]
        public string PhoneNumber { get; set; }

        public string VerificationCode { get; set; }
        public string Address { get; set; }
    }
}