using System;
using Journalist;
using Journalist.Collections;
using LodCoreLibraryOld.Domain.UserManagement;

namespace LodCoreApiOld.Models
{
    public class Developer
    {
        public Developer(
            int userId,
            string firstName,
            string lastName,
            string email,
            ConfirmationStatus confirmationStatus,
            bool isOauthRegistered,
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
            Require.NotNull(isOauthRegistered, nameof(isOauthRegistered));

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ConfirmationStatus = confirmationStatus;
            IsOauthRegistered = isOauthRegistered;
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

        public int UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public ConfirmationStatus ConfirmationStatus { get; }

        public bool IsOauthRegistered { get; }

        public Uri PhotoUri { get; }

        public DateTime RegistrationDate { get; }

        public Uri LinkToGithubProfile { get; set; }

        public Uri VkProfileUri { get; }

        public string PhoneNumber { get; }

        public int? StudentAccessionYear { get; }

        public bool IsGraduated { get; }

        public string StudyingDirection { get; }

        public string InstituteName { get; }

        public string Specialization { get; }

        public string Role { get; }

        public DeveloperPageProjectPreview[] Projects { get; }
    }
}