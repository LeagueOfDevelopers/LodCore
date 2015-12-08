using Common;
using UserManagement.Infrastructure;

namespace UserManagement.Application
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(string email, string lastname, string firstname, Password password)
        {
            Email = Validator.GetValidEmail(email);
            Lastname = lastname;
            Firstname = firstname;
            Password = password;
        }

        public string Email { get; private set; }

        public string Lastname { get; private set; }

        public string Firstname { get; private set; }

        public Password Password { get; private set; }
    }
}