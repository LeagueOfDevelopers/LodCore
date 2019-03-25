using System;
using Journalist;

namespace LodCoreApiOld.Models
{
    public class GuestDeveloper
    {
        public GuestDeveloper(
            int userId,
            string firstName,
            string lastName,
            Uri photoUri,
            DateTime registrationDate,
            Uri vkProfileUri,
            Uri linkToGithubProfile,
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

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhotoUri = photoUri;
            RegistrationDate = registrationDate;
            VkProfileUri = vkProfileUri;
            LinkToGithubProfile = linkToGithubProfile;
            StudentAccessionYear = studentAccessionYear;
            IsGraduated = isGraduated;
            StudyingDirection = studyingDirection;
            InstituteName = instituteName;
            Specialization = specialization;
            Role = role;
            Projects = projects;
        }

        public int UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public Uri PhotoUri { get; }

        public DateTime RegistrationDate { get; }

        public Uri VkProfileUri { get; }

        public Uri LinkToGithubProfile { get; }

        public int? StudentAccessionYear { get; }

        public bool IsGraduated { get; }

        public string StudyingDirection { get; }

        public string InstituteName { get; }

        public string Specialization { get; }

        public string Role { get; }

        public DeveloperPageProjectPreview[] Projects { get; }
    }
}