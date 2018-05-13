using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using UserManagement.Infrastructure;
using NotificationService;
using Serilog;
using System;

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
            var client = _mailerSettings.GetSmtpClientRelayer();

            mail.Subject = MailingResources.ConfirmationMailCaption;
            mail.Body = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.EmailConfirmation(userName, confirmationLink));
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);
                Log.Information("Mail with event type of EmailConfirmation has sent to {@0}", mail.To);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    Log.Error(ex, "Failed to deliver mail to {@0}: {1}. ExceptionStatusCode: {2}", mail.To, ex.Message, ex.InnerExceptions[i].StatusCode);
                    client.Send(mail);
                    Log.Information("Retry: mail with event type of EmailConfirmation has sent to {@1}", mail.To);
                }
            }
            catch (SmtpException ex)
            {
                Log.Error(ex, "Failed to send mail to {@0}: {1}. StackTrace: {2}", mail.To, ex.Message, ex.StackTrace);
            }
            finally
            {
                client.Dispose();
            }
        }

        public void SendPasswordResetMail(string userName, string resetLink, MailAddress emailAddress)
        {
            Require.NotNull(resetLink, nameof(resetLink));
            Require.NotNull(emailAddress, nameof(emailAddress));

            var mail = InitMail(emailAddress);
            var client = _mailerSettings.GetSmtpClientRelayer();

            mail.Subject = MailingResources.PasswordResetCaption;
            mail.Body = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.PasswordRecovery(userName, resetLink));
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);
                Log.Information("Mail with event type of PasswordRecovery has sent to {@0}", mail.To);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    Log.Error(ex, "Failed to deliver mail to {@0}: {1}. ExceptionStatusCode: {2}", mail.To, ex.Message, ex.InnerExceptions[i].StatusCode);
                    client.Send(mail);
                    Log.Information("Retry: mail with event type of PasswordRecovery has sent to {@1}", mail.To);
                }
            }
            catch (SmtpException ex)
            {
                Log.Error(ex, "Failed to send mail to {@0}: {1}. StackTrace: {2}", mail.To, ex.Message, ex.StackTrace);
            }
            finally
            {
                client.Dispose();
            }
        }

        public void SendNotificationEmail(int[] userIds, IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));
            Require.NotNull(userIds, nameof(userIds));

            var mail = new MailMessage
            {
                From = new MailAddress(_mailerSettings.From, _mailerSettings.DisplayName)
            };
            var client = _mailerSettings.GetSmtpClientRelayer();

            mail.Subject = MailingResources.NotificationMailCaption;
            mail.IsBodyHtml = true;

            try
            {
                foreach (var user in userIds.Select(userId => _usersRepository.GetAccount(userId)))
                {
                    mail.Body = _notificationEmailDescriber.Describe(user.Firstname, eventInfo);
                    mail.To.Add(user.Email);

                    try
                    {
                        client.Send(mail);
                        Log.Information("Mail with event type of {0} has sent to {@1}", eventInfo.GetEventType(), mail.To);
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            Log.Error(ex, "Failed to deliver mail to {@0}: {1}. ExceptionStatusCode: {2}", mail.To, ex.Message, ex.InnerExceptions[i].StatusCode);
                            client.Send(mail);
                            Log.Information("Retry: mail with event type of {0} has sent to {@1}", eventInfo.GetEventType(), mail.To);
                        }
                    }
                    catch (SmtpException ex)
                    {
                        Log.Error(ex, "Failed to send mail to {@0}: {1}. StackTrace: {2}", mail.To, ex.Message, ex.StackTrace);
                        _eventPublisher.PublishEvent((EventInfoBase)eventInfo);
                    }

                    mail.To.Clear();
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Failed to send mail. An error occured: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
            }
            client.Dispose();
        }
        
        private MailMessage InitMail(MailAddress emailAddress)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_mailerSettings.From, _mailerSettings.DisplayName);
            mail.To.Add(emailAddress);
            return mail;
        }

        private readonly MailerSettings _mailerSettings;
        private readonly INotificationEmailDescriber _notificationEmailDescriber;
        private readonly IUserRepository _usersRepository;
        private readonly IEventPublisher _eventPublisher;
    }
}