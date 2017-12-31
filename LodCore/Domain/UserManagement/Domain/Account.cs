using System;
using System.Net.Mail;
using Common;
using Journalist;

namespace UserManagement.Domain
{
    public class Account
    {
        public Account(
            string firstname,
            string lastname,
            MailAddress email,
            Password password,
            AccountRole role,
            ConfirmationStatus confirmationStatus,
            DateTime registrationTime,
            Profile profile)
        {
            Require.NotEmpty(firstname, nameof(firstname));
            Require.NotEmpty(lastname, nameof(lastname));
            Require.NotNull(email, nameof(email));
            Require.NotNull(password, nameof(password));
            Require.NotNull(email, nameof(email));
            Require.NotNull(password, nameof(password));

            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            Role = role;
            ConfirmationStatus = confirmationStatus;
            RegistrationTime = registrationTime;
            Profile = profile;
        }

        protected Account()
        {
        }

        public virtual int UserId { get; protected set; }

        public virtual string Firstname { get; protected set; }

        public virtual string Lastname { get; protected set; }

        public virtual MailAddress Email { get; protected set; }

        public virtual Password Password { get; set; }

        public virtual bool IsHidden { get; set; }

        public virtual AccountRole Role { get; set; }

        public virtual ConfirmationStatus ConfirmationStatus { get; set; }

        public virtual DateTime RegistrationTime { get; protected set; }

        public virtual Profile Profile { get; set; }
    }
}