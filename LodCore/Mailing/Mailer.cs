using System.Net;
using System.Net.Mail;
using Journalist;
using NotificationService;
using UserManagement.Infrastructure;
using IMailer = UserManagement.Application.IMailer;

namespace Mailing
{
    public class Mailer : IMailer, NotificationService.IMailer
    {
        private readonly MailerSettings _mailerSettings;

        private readonly INotificationEmailDescriber _notificationEmailDescriber;

        private readonly IUserRepository _usersRepository;

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

        public void SendConfirmationMail(string confirmationToken, MailAddress emailAddress)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = InitClient();

            mail.Subject = MailingResources.ConfirmationMailCaption;
            mail.Body = string.Format(MailingResources.ConfirmationMessageTemplate, confirmationToken);

            client.Send(mail);
            client.Dispose();
        }

        public void SendNotificationEmail(int[] userIds, IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));
            Require.NotNull(userIds, nameof(userIds));

            var mail = new MailMessage();
            var client = InitClient();

            mail.Subject = MailingResources.NotificationMailCaption;
            mail.Body = string.Format(MailingResources.NotificationMessageTemplate,
                _notificationEmailDescriber.Describe((dynamic) eventInfo));

            foreach (var userId in userIds)
            {
                var emailAdress = _usersRepository.GetAccount(userId).Email;
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

        private SmtpClient InitClient()
        {
            var client = new SmtpClient();
            client.Host = _mailerSettings.SmtpServer;
            client.Port = _mailerSettings.Port;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_mailerSettings.From, _mailerSettings.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}