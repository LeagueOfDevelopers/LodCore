using System;
using System.ComponentModel.DataAnnotations;

namespace LodCoreApiOld.Models
{
    public class ProfileUpdateRequest
    {
        public LodCoreLibraryOld.Common.Image Image { get; set; }

        [MaxLength(100)]
        [RegularExpression("^(http|https)?://vk.com/.+$")]
        public string VkProfileUri { get; set; }

        public Uri LinkToGithubProfile { get; set; }

        [MaxLength(11)]
        [RegularExpression(@"^7\d{10}$")]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [Range(typeof(int), "2000", "2030")] public int? StudentAccessionYear { get; set; }

        public bool IsGraduated { get; set; }

        [MaxLength(255)] public string StudyingDirection { get; set; }

        [MaxLength(10)] public string InstituteName { get; set; }

        [MaxLength(255)] public string Specialization { get; set; }
    }
}