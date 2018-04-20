namespace Guap.Server.Models
{
    using System.ComponentModel.DataAnnotations;

    public class NotificationModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public bool NotificationsEnabled { get; set; }
    }
}