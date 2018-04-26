using System.ComponentModel.DataAnnotations;

namespace Guap.Server.Data.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; }
        
        [StringLength(42)]
        public string Address { get; set; }

        public string VerificationCode { get; set; }
        
        [StringLength(253)]
        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }

        public bool NotificationsEnabled { get; set; } = true;
    }
}