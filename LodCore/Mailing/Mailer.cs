﻿using System.Net;
using System.Net.Mail;
using Journalist;
using UserManagement.Application;
using NotificationService;

namespace Mailing
{
    public class Mailer : UserManagement.Application.IMailer, NotificationService.IMailer
    {
        private readonly MailerSettings _mailerSettings;

        public Mailer(MailerSettings mailerSettings)
        {
            _mailerSettings = mailerSettings;
        }

        public void SendConfirmationMail(string confirmationToken, MailAddress emailAddress)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));
            Require.NotNull(emailAddress, nameof(emailAddress));

            MailMessage mail = InitMail(emailAddress);

            mail.Subject = _mailerSettings.CaptionForConfirmation;
            mail.Body = string.Format(_mailerSettings.ConfirmationMessageTemplate, confirmationToken);

            SendMail(mail);
        }

        public void SendNotificationEmail(MailAddress emailAddress, IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));
            Require.NotNull(emailAddress, nameof(emailAddress));

            MailMessage mail = InitMail(emailAddress);

            mail.Subject = _mailerSettings.CaptionForNotification;
            mail.Body = string.Format(_mailerSettings.NotificationMessageTemplate, "Офигенное событие!");

            SendMail(mail);
        }

        private void SendMail(MailMessage mail)
        {
            var client = new SmtpClient();
            client.Host = _mailerSettings.SmtpServer;
            client.Port = _mailerSettings.Port;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_mailerSettings.From, _mailerSettings.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
            mail.Dispose();
        }

        private MailMessage InitMail(MailAddress email)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_mailerSettings.From);
            mail.To.Add(email);
            return mail;
        }
    }
}