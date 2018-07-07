using System.ComponentModel.DataAnnotations;

namespace LodCoreApi.Models
{
    public class RegisterDeveloperWithGithubRequest
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string VkProfileUri { get; set; }

        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string StudyingProfile { get; set; }

        [MaxLength(10)]
        public string InstituteName { get; set; }

        [MaxLength(255)]
        public string Department { get; set; }

        public int AccessionYear { get; set; }

        public bool IsGraduated { get; set; }
    }
}