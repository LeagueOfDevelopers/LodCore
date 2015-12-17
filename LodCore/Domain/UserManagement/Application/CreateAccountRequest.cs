using System.Net.Mail;

namespace UserManagement.Application
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(MailAddress email, string lastname, string firstname, string password)
        {
            Email = email.Address;
            Lastname = lastname;
            Firstname = firstname;
            Password = password;
        }

        public string Email { get; private set; }

        public string Lastname { get; private set; }

        public string Firstname { get; private set; }

        public string Password { get; private set; }
    }
}