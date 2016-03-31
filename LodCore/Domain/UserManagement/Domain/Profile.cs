using System;
using Common;

namespace UserManagement.Domain
{
    public class Profile
    {
        public virtual Image Image { get; set; }

        public virtual Uri VkProfileUri { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual int? StudentAccessionYear { get; set; }

        public virtual string StudyingDirection { get; set; }

        public virtual string InstituteName { get; set; }

        public virtual string Specialization { get; set; }
    }
}