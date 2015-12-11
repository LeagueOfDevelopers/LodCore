using System;
using System.Net.Mail;
using Journalist;

namespace UserManagement.Domain
{
    public class Profile
    {
        public Profile(
            int userId,
            Uri bigPhotoUri,
            Uri smallPhotoUri,
            MailAddress email,
            DateTime registrationTime,
            Uri vkProfileUri,
            string phoneNumber,
            int studentAccessionYear,
            string studyingDirection,
            string instituteName,
            string specialization)
        {
            Require.ZeroOrGreater(userId, nameof(userId));
            Require.NotNull(registrationTime, nameof(registrationTime));
            Require.NotNull(email, nameof(email));
            Require.NotEmpty(phoneNumber, nameof(phoneNumber));

            UserId = userId;
            BigPhotoUri = bigPhotoUri;
            SmallPictureUri = smallPhotoUri;
            RegistrationTime = registrationTime;
            VkProfileUri = vkProfileUri;
            PhoneNumber = phoneNumber;
            StudentAccessionYear = studentAccessionYear;
            StudyingDirection = studyingDirection;
            InstituteName = instituteName;
            Specialization = specialization;
        }

        protected Profile()
        {
        }

        public virtual int UserId { get; protected set; }

        public virtual Uri BigPhotoUri { get; protected set; }

        public virtual Uri SmallPictureUri { get; protected set; }

        public virtual DateTime RegistrationTime { get; protected set; }

        public virtual Uri VkProfileUri { get; protected set; }

        public virtual string PhoneNumber { get; protected set; }

        public virtual int StudentAccessionYear { get; set; }

        public virtual string StudyingDirection { get; set; }

        public virtual string InstituteName { get; set; }

        public virtual string Specialization { get; set; }
    }
}