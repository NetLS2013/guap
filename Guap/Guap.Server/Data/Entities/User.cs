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
        
        [StringLength(40)]
        public string Address { get; set; }

        public string VerificationCode { get; set; }
    }
}