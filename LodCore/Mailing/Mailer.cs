using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using UserManagement.Infrastructure;
using NotificationService;
using Serilog;

namespace Mailing
{
    public class Mailer : IMailer
    {
        public Mailer(MailerSettings mailerSettings, 
            INotificationEmailDescriber notificationEmailDescriber,
            IUserRepository usersRepository,
            IEventPublisher eventPublisher)
        {
            Require.NotNull(mailerSettings, nameof(mailerSettings));
            Require.NotNull(notificationEmailDescriber, nameof(notificationEmailDescriber));
            Require.NotNull(usersRepository, nameof(usersRepository));
            Require.NotNull(eventPublisher, nameof(eventPublisher));

            _mailerSettings = mailerSettings;
            _notificationEmailDescriber = notificationEmailDescriber;
            _usersRepository = usersRepository;
            _eventPublisher = eventPublisher;
        }

        public void SendConfirmationMail(string userName, string confirmationLink, MailAddress emailAddress)
        {
            Require.NotNull(confirmationLink, nameof(confirmationLink));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.ConfirmationMailCaption;
            mail.Body = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.EmailConfirmation(userName, confirmationLink));
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);
            }
            catch (SmtpException ex)
            {
                Log.Error(ex, "Sending mail failure: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
            }
            Log.Information("Mail with subject {0} has sent to {@1}", mail.Subject, mail.To);
            client.Dispose();
        }

        public void SendPasswordResetMail(string userName, string resetLink, MailAddress emailAddress)
        {
            Require.NotNull(resetLink, nameof(resetLink));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = _mailerSettings.GetSmtpClient();

            mail.Subject = MailingResources.PasswordResetCaption;
            mail.Body = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.PasswordRecovery(userName, resetLink));
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);
            }
            catch (SmtpException ex)
            {
                Log.Error(ex, "Sending mail failure: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
            }
            Log.Information("Mail with subject {0} has sent to {@1}", mail.Subject, mail.To);
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
            mail.IsBodyHtml = true;

            foreach (var user in userIds.Select(userId => _usersRepository.GetAccount(userId)))
            {
                mail.Body = _notificationEmailDescriber.Describe(user.Firstname, eventInfo);
                mail.To.Add(user.Email);

                try
                {
                    client.Send(mail);
                }
                catch (SmtpException ex)
                {
                    Log.Error(ex, "Sending mail failure: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
                    _eventPublisher.PublishEvent((EventInfoBase)eventInfo);
                }
                Log.Information("Mail with subject {0} has sent to {@1}", mail.Subject, mail.To);
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
        private readonly IEventPublisher _eventPublisher;
    }
}