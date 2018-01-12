using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using UserManagement.Infrastructure;
using IMailer = UserManagement.Application.IMailer;

namespace Mailing
{
    public class Mailer : IMailer
    {
        public Mailer(MailerSettings mailerSettings, INotificationEmailDescriber notificationEmailDescriber,
            IUserRepository usersRepository)
        {
            Require.NotNull(mailerSettings, nameof(mailerSettings));
            Require.NotNull(notificationEmailDescriber, nameof(notificationEmailDescriber));
            Require.NotNull(usersRepository, nameof(usersRepository));

            _mailerSettings = mailerSettings;
            _notificationEmailDescriber = notificationEmailDescriber;
            _usersRepository = usersRepository;
        }

        public void SendConfirmationMail(string confirmationLink, MailAddress emailAddress)
        {
            Require.NotNull(confirmationLink, nameof(confirmationLink));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.ConfirmationMailCaption;
            mail.Body = string.Format(MailingResources.ConfirmationMessageTemplate, confirmationLink);

            client.Send(mail);
            client.Dispose();
        }

        public void SendPasswordResetMail(string resetLink, MailAddress emailAddress)
        {
            Require.NotNull(resetLink, nameof(resetLink));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.PasswordResetCaption;
            mail.Body = string.Format(MailingResources.PasswordResetMessageTemplate, resetLink);

            client.Send(mail);
            client.Dispose();
        }

        public void SendNotificationEmail(int[] userIds, IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));
            Require.NotNull(userIds, nameof(userIds));

            var mail = new MailMessage
            {
                From = new MailAddress(_mailerSettings.From)
            };
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.NotificationMailCaption;
            mail.Body = string.Format(MailingResources.NotificationMessageTemplate,
                _notificationEmailDescriber.Describe((dynamic) eventInfo));

            foreach (var emailAdress in userIds.Select(userId => _usersRepository.GetAccount(userId).Email))
            {
                mail.To.Add(emailAdress);

                client.Send(mail);

                mail.To.Clear();
            }

            mail.Dispose();
        }
        
        private MailMessage InitMail(MailAddress emailAddress)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_mailerSettings.From);
            mail.To.Add(emailAddress);
            return mail;
        }

        private readonly MailerSettings _mailerSettings;
        private readonly INotificationEmailDescriber _notificationEmailDescriber;
        private readonly IUserRepository _usersRepository;
    }
}