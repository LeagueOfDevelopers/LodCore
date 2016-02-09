using System;

namespace UserManagement.Domain
{
    public class Profile
    {
        public virtual int UserId { get; protected set; }

        public virtual Uri BigPhotoUri { get; set; }

        public virtual Uri SmallPhotoUri { get; set; }

        public virtual Uri VkProfileUri { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual int? StudentAccessionYear { get; set; }

        public virtual string StudyingDirection { get; set; }

        public virtual string InstituteName { get; set; }

        public virtual string Specialization { get; set; }
    }
}