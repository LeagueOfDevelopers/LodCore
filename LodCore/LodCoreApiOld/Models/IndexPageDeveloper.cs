using System;

namespace LodCoreApiOld.Models
{
    public class IndexPageDeveloper
    {
        public IndexPageDeveloper(
            int userId,
            string firstName,
            string lastName,
            Uri photoUri,
            string role)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhotoUri = photoUri;
            Role = role;
        }

        public int UserId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public Uri PhotoUri { get; }

        public string Role { get; }
    }
}