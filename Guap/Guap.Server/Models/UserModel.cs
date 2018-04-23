using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Models
{
    public class UserModel
    {
        [RegularExpression(@"^\+?[1-9]\d{1,14}$",
            ErrorMessage = "Please enter valid phone number.")]
        public string PhoneNumber { get; set; }

        public string VerificationCode { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}