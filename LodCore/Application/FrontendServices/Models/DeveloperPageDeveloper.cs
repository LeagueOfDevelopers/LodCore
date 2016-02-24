using System;
using Journalist;

namespace FrontendServices.Models
{
    public class DeveloperPageDeveloper
    {
        public DeveloperPageDeveloper(
            int userId, 
            string firstName, 
            string lastName,
            Uri photoUri, 
            string role,
            DateTime registrationDate, 
            int projectCount, 
            Uri vkPageUri)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));
            Require.NotEmpty(role, nameof(role));
            Require.ZeroOrGreater(projectCount, nameof(projectCount));

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhotoUri = photoUri;
            Role = role;
            RegistrationDate = registrationDate;
            ProjectCount = projectCount;
            VkPageUri = vkPageUri;
        }

        public int UserId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Uri PhotoUri { get; private set; }

        public string Role { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public int ProjectCount { get; private set; }

        public Uri VkPageUri { get; private set; }
    }
}