using System;
using Journalist;

namespace UserManagement
{
    public class User
    {
        public User(
            UserSummary userSummary, 
            Uri bigPhotoUri, 
            string email, 
            DateTimeOffset registrationTime, 
            Uri vkProfileUri, 
            string phoneNumber, 
            uint studentAccessionYear, 
            string studyingDirection, 
            string instituteName,
            string specialization)
        {
            Require.NotNull(userSummary, nameof(userSummary));
            Require.NotNull(registrationTime, nameof(registrationTime));
            Require.NotEmpty(email, nameof(email));
            Require.NotEmpty(phoneNumber, nameof(phoneNumber));

            UserSummary = userSummary;
            BigPhotoUri = bigPhotoUri;
            Email = email;
            RegistrationTime = registrationTime;
            VkProfileUri = vkProfileUri;
            PhoneNumber = phoneNumber;
            StudentAccessionYear = studentAccessionYear;
            StudyingDirection = studyingDirection;
            InstituteName = instituteName;
            Specialization = specialization;
        }

        public UserSummary UserSummary { get; private set; }
        
        public Uri BigPhotoUri { get; private set; }

        public string Email { get; private set; }

        public DateTimeOffset RegistrationTime { get; private set; }

        public Uri VkProfileUri { get; private set; }

        public string PhoneNumber { get; private set; }

        public uint StudentAccessionYear { get; private set; }

        public string StudyingDirection { get; private set; }

        public string InstituteName { get; private set; }

        public string Specialization { get; private set; }
    }
}
