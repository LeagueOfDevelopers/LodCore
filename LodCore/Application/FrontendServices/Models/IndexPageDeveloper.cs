using System;

namespace FrontendServices.Models
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

        public int UserId { get; private set; }
         
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Uri PhotoUri { get; private set; }

        public string Role { get; private set; }
    }
}