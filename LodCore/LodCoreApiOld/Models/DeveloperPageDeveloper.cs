using System;
using Journalist;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Pagination;

namespace LodCoreApiOld.Models
{
    public class DeveloperPageDeveloper : IPaginable
    {
        public DeveloperPageDeveloper(
            int userId,
            string firstName,
            string lastName,
            Uri photoUri,
            string role,
            DateTime registrationDate,
            int projectCount,
            Uri vkPageUri,
            AccountRole accountRole,
            ConfirmationStatus confirmationStatus,
            bool isHidden)
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
            AccountRole = accountRole;
            ConfirmationStatus = confirmationStatus;
            IsHidden = isHidden;
        }

        public int UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public Uri PhotoUri { get; }

        public string Role { get; }

        public DateTime RegistrationDate { get; }

        public int ProjectCount { get; }

        public Uri VkPageUri { get; }

        public AccountRole AccountRole { get; }

        public ConfirmationStatus ConfirmationStatus { get; }

        public bool IsHidden { get; }
    }
}