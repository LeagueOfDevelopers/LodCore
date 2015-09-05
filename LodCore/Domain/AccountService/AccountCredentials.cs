using Journalist;

namespace AccountService
{
    public class AccountCredentials
    {
        public AccountCredentials(uint userId, string email, string passwordHash)
        {
            Require.NotEmpty(email, nameof(email));
            Require.NotEmpty(passwordHash, nameof(passwordHash));

            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
        }

        public uint UserId { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; private set; }
    }
}