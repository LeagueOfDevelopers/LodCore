using UserManagement.Domain;

namespace FrontendServices.Models
{
    public class UpdateProfileRequest
    {
        public string NewPassword { get; set; }

        public Profile Profile { get; set; }
    }
}