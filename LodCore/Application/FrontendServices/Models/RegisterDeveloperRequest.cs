using System.ComponentModel.DataAnnotations;

namespace FrontendServices.Models
{
    public class RegisterDeveloperRequest
    {
        [EmailAddress]
        [RegularExpression(@"\b[a-z0-9.%-]+@[a-z0-9.-]+.[a-z]{2,4}\b")]
        public string Email { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"^[А-Яа-яёЁ]+$")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"^[А-Яа-яёЁ]+$")]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(100)]
        [RegularExpression("^http://vk.com/[a-zA-Z0-9]+$")]
        public string VkProfileUri { get; set; }

        [MaxLength(11)]
        [RegularExpression(@"^7\d{10}$")]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        [RegularExpression(@"^[А-Яа-яёЁ\s]+$")]
        public string StudyingProfile { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"^[А-Яа-яёЁ]+$")]
        public string InstituteName { get; set; }

        [MaxLength(255)]
        [RegularExpression(@"^[А-Яа-яёЁ\s]+$")]
        public string Department { get; set; }

        [Range(typeof(int), "2000", "2030")]
        public int AccessionYear { get; set; }

        public bool IsGraduated { get; set; }
    }
}