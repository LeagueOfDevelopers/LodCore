using System.Linq;
using System.Net;
using System.Net.Mail;
using Journalist;
using NotificationService;
using OrderManagement.Infrastructure;
using UserManagement.Application;
using IMailer = UserManagement.Application.IMailer;

namespace Mailing
{
    public class Mailer : IMailer, NotificationService.IMailer
    {
        private readonly MailerSettings _mailerSettings;

        private readonly INotificationEmailDescriber _notificationEmailDescriber;

        private readonly IUserManager _userManager;

        public Mailer(MailerSettings mailerSettings, INotificationEmailDescriber notificationEmailDescriber, IUserManager userManager)
        {
            _mailerSettings = mailerSettings;
            _notificationEmailDescriber = notificationEmailDescriber;
            _userManager = userManager;
        }

        public void SendConfirmationMail(string confirmationToken, MailAddress emailAddress)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = InitClient();

            mail.Subject = _mailerSettings.CaptionForConfirmation;
            mail.Body = string.Format(_mailerSettings.ConfirmationMessageTemplate, confirmationToken);

            client.Send(mail);
            client.Dispose();
        }

        public void SendNotificationEmail(int[] userIds, IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));
            Require.NotNull(userIds, nameof(userIds));

            var emailsToSend = _userManager.GetUserList(acc => userIds.Contains(acc.UserId)).Select(a => a.Email).ToArray();

            var mail = InitMail(emailsToSend);
            var client = InitClient();

            mail.Subject = _mailerSettings.CaptionForNotification;
            mail.Body = string.Format(_mailerSettings.NotificationMessageTemplate, _notificationEmailDescriber.Describe((dynamic)eventInfo));

            client.Send(mail);
            mail.Dispose();
        }

        private MailMessage InitMail(MailAddress[] emailAddresses)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_mailerSettings.From);
            foreach (var emailAddress in emailAddresses)
            {
                mail.To.Add(emailAddress);
            }
            return mail;
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