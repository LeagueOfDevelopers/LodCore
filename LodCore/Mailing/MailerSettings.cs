using System;

namespace Mailing
{
    public class MailerSettings
    {
        public MailerSettings()
        {
            CaptionForNotification = MailingResources.NotificationMailCaption;
            SmtpServer = MailingResources.SmtpServer;
            Port = Convert.ToInt16(MailingResources.Port);
            Password = MailingResources.Password;
            From = MailingResources.Email;
            CaptionForConfirmation = MailingResources.ConfirmationMailCaption;
            ConfirmationMessageTemplate = MailingResources.ConfirmationMessageTemplate;
            NotificationMessageTemplate = MailingResources.NotificationMessageTemplate;

        }

        public string SmtpServer { get; }
        public int Port { get; }
        public string Password { get; }

        public string From { get; }

        public string ConfirmationMessageTemplate { get; }
        public string CaptionForConfirmation { get; }

        public string NotificationMessageTemplate { get; }
        public string CaptionForNotification { get; }
    }
}