using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Journalist;
using UserManagement.Application;

namespace UserManagement.Infrastructure
{
    class Mailer : IMailer
    {
        public void SendConfirmationMail(string confirmationToken, string email)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));
            Require.NotNull(email, nameof(email));

            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
            string password = ConfigurationManager.AppSettings["password"];

            string from = ConfigurationManager.AppSettings["from"];
            string message = string.Format(ValidationMessageResources.messageTemplate, confirmationToken);
            string caption = ConfigurationManager.AppSettings["caption"];

            var mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(email));
            mail.Subject = caption;
            mail.Body = message;
            var client = new SmtpClient();
            client.Host = smtpServer;
            client.Port = port;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from.Split('@')[0], password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
            mail.Dispose();
        }
    }
}
