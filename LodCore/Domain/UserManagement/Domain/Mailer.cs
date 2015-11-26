using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using UserManagement.Application;
using Journalist;

namespace UserManagement.Domain
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
            string message1 = ConfigurationManager.AppSettings["message1"];
            string message2 = ConfigurationManager.AppSettings["message2"];
            string caption = ConfigurationManager.AppSettings["caption"];



            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(email));
            mail.Subject = caption;
            mail.Body = message1 + confirmationToken + message2;
            SmtpClient client = new SmtpClient();
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
