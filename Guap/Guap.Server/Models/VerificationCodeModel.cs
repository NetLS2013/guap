using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Models
{
    public class VerificationCodeModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string VerificationCode { get; set; }
    }
}