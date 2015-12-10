using Journalist;

namespace UserManagement.Domain
{
    public class Account
    {
        public Account(
            string firstname,
            string lastname,
            string email,
            string passwordHash,
            AccountRole role,
            ConfirmationStatus confirmationStatus,
            Profile profile, 
            int redmineUserId, 
            int gitlabUserId)
        {
            Require.NotEmpty(firstname, nameof(firstname));
            Require.NotEmpty(lastname, nameof(lastname));
            Require.NotEmpty(email, nameof(email));
            Require.NotEmpty(passwordHash, nameof(passwordHash));
            Require.Positive(redmineUserId, nameof(redmineUserId));
            Require.Positive(gitlabUserId, nameof(gitlabUserId));

            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            ConfirmationStatus = confirmationStatus;
            Profile = profile;
            RedmineUserId = redmineUserId;
            GitlabUserId = gitlabUserId;
        }

        protected Account()
        {
        }

        public virtual int UserId { get; protected set; }

        public virtual string Firstname { get; protected set; }

        public virtual string Lastname { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual string PasswordHash { get; protected set; }

        public virtual int RedmineUserId { get; protected set; }

        public virtual int GitlabUserId { get; protected set; }

        public virtual AccountRole Role { get; protected set; }

        public virtual ConfirmationStatus ConfirmationStatus { get; set; }

        public virtual Profile Profile { get; protected set; }
    }
}