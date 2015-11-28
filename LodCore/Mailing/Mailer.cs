using System.Net;
using System.Net.Mail;
using Journalist;
using UserManagement.Application;

namespace Mailing
{
    public class Mailer : IMailer
    {
        private readonly MailerSettings _mailerSettings;

        public Mailer(MailerSettings mailerSettings)
        {
            _mailerSettings = mailerSettings;
        }

        public void SendConfirmationMail(string confirmationToken, string email)
        {
            Require.NotNull(confirmationToken, nameof(confirmationToken));
            Require.NotNull(email, nameof(email));

            //var smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            //var port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
            //var password = ConfigurationManager.AppSettings["password"];

            //var from = ConfigurationManager.AppSettings["from"];
            //var message = string.Format(ValidationMessageResources.messageTemplate, confirmationToken);
            //var caption = ConfigurationManager.AppSettings["caption"];

            var mail = new MailMessage();
            mail.From = new MailAddress(_mailerSettings.From);
            mail.To.Add(new MailAddress(email));
            mail.Subject = _mailerSettings.Caption;
            mail.Body = string.Format(_mailerSettings.MessageTemplate, confirmationToken);

            var client = new SmtpClient();
            client.Host = _mailerSettings.SmtpServer;
            client.Port = _mailerSettings.Port;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_mailerSettings.From, _mailerSettings.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
            mail.Dispose();
        }
    }
}