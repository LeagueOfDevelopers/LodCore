using System;
using Journalist;
using Journalist.Collections;
using UserManagement.Domain;

namespace FrontendServices.Models
{
    public class Developer
    {
        public Developer(
            int userId,
            string firstName,
            string lastName,
            string email,
            ConfirmationStatus confirmationStatus,
            Uri photoUri,
            DateTime registrationDate,
            Uri linkToGithubProfile,
            Uri vkProfileUri,
            string phoneNumber,
            int? studentAccessionYear,
            bool isGraduated,
            string studyingDirection,
            string instituteName,
            string specialization,
            string role,
            DeveloperPageProjectPreview[] projects)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));
            Require.NotEmpty(email, nameof(email));

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ConfirmationStatus = confirmationStatus;
            PhotoUri = photoUri;
            RegistrationDate = registrationDate;
            LinkToGithubProfile = linkToGithubProfile;
            VkProfileUri = vkProfileUri;
            PhoneNumber = phoneNumber;
            StudentAccessionYear = studentAccessionYear;
            IsGraduated = isGraduated;
            StudyingDirection = studyingDirection;
            InstituteName = instituteName;
            Specialization = specialization;
            Role = role;
            Projects = projects ?? EmptyArray.Get<DeveloperPageProjectPreview>();
        }

        public int UserId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public ConfirmationStatus ConfirmationStatus { get; private set; }

        public Uri PhotoUri { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public Uri LinkToGithubProfile { get; set; }

        public Uri VkProfileUri { get; private set; }

        public string PhoneNumber { get; private set; }

        public int? StudentAccessionYear { get; private set; }

        public bool IsGraduated { get; private set; }

        public string StudyingDirection { get; private set; }

        public string InstituteName { get; private set; }

        public string Specialization { get; private set; }

        public string Role { get; private set; }

        public DeveloperPageProjectPreview[] Projects { get; private set; }
    }
}