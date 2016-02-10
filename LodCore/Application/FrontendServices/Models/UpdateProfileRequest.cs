using System.ComponentModel.DataAnnotations;
using UserManagement.Domain;

namespace FrontendServices.Models
{
    public class UpdateProfileRequest
    {
        [MaxLength(50)]
        public string NewPassword { get; set; }

        public Profile Profile { get; set; }

        public NotificationSetting[] NotificationSettings { get; set; }
    }
}