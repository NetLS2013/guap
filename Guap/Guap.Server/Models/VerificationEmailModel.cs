using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Models
{
    public class VerificationEmailModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string VerificationCode { get; set; }
    }
}