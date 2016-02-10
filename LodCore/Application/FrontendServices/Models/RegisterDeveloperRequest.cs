using System.ComponentModel.DataAnnotations;

namespace FrontendServices.Models
{
    public class RegisterDeveloperRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string VkProfileUri { get; set; }
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string StudyingProfile { get; set; }
        [MaxLength(10)]
        public string InstituteName { get; set; }
        [MaxLength(30)]
        public string Department { get; set; }
        
        public int AccessionYear { get; set; }
    }
}