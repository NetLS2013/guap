using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Models
{
    public class VerificationEmailModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string VerificationCode { get; set; }
    }
}