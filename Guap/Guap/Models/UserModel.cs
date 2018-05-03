namespace Guap.Models
{
    public class UserModel
    {
        public string PhoneNumber { get; set; }
        
        public string VerificationCode { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string Pin { get; set; }
    }
}