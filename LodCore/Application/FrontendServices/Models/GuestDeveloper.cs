using System;
using Journalist;

namespace FrontendServices.Models
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

        public int UserId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Uri PhotoUri { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public Uri VkProfileUri { get; private set; }

        public Uri LinkToGithubProfile { get; private set; }

        public int? StudentAccessionYear { get; private set; }

        public bool IsGraduated { get; private set; }

        public string StudyingDirection { get; private set; }

        public string InstituteName { get; private set; }

        public string Specialization { get; private set; }

        public string Role { get; private set; }

        public DeveloperPageProjectPreview[] Projects { get; private set; }
    }
}