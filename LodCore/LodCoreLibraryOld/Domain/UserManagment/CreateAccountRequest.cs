using System.Net.Mail;
using Journalist;

namespace LodCoreLibraryOld.Domain.UserManagement
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(
            MailAddress email, 
            string lastname, 
            string firstname, 
            string password, 
            Profile profile)
        {
            Require.NotNull(email, nameof(email));
            Require.NotEmpty(lastname, nameof(lastname));
            Require.NotEmpty(firstname, nameof(firstname));
            Require.NotEmpty(password, nameof(password));

            Email = email.Address;
            Lastname = lastname;
            Firstname = firstname;
            Password = password;
            Profile = profile;
        }

        public CreateAccountRequest(
            string lastname,
            string firstname,
            Profile profile)
        {
            Require.NotEmpty(lastname, nameof(lastname));
            Require.NotEmpty(firstname, nameof(firstname));

            Lastname = lastname;
            Firstname = firstname;
            Profile = profile;
        }

        public string Email { get; private set; }

        public string Lastname { get; private set; }

        public string Firstname { get; private set; }

        public string Password { get; private set; }

        public Profile Profile { get; private set; }
    }
}