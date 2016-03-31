using System;

namespace FrontendServices.Models
{
    public class ProfileUpdateRequest
    {
        public Uri ImageUri { get; set; }

        public Uri VkProfileUri { get; set; }

        public string PhoneNumber { get; set; }

        public int? StudentAccessionYear { get; set; }

        public string StudyingDirection { get; set; }

        public string InstituteName { get; set; }

        public string Specialization { get; set; }
    }
}