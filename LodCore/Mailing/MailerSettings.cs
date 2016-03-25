using System;
using System.Net;
using System.Net.Mail;
using Journalist;

namespace Mailing
{
    public class MailerSettings
    {
        public MailerSettings(
            string smtpServer, 
            int port, 
            string password, 
            string @from, 
            string caption, 
            string messageTemplate, 
            int basicEmailTimeoutInSecond, 
            int timeoutIncrementInSeconds)
        {
            Require.NotEmpty(smtpServer, nameof(smtpServer));
            Require.Positive(port, nameof(port));
            Require.NotEmpty(password, nameof(password));
            Require.NotEmpty(@from, nameof(@from));
            Require.NotEmpty(caption, nameof(caption));
            Require.NotEmpty(messageTemplate, nameof(messageTemplate));
            Require.Positive(basicEmailTimeoutInSecond, nameof(basicEmailTimeoutInSecond));
            Require.Positive(timeoutIncrementInSeconds, nameof(timeoutIncrementInSeconds));

            SmtpServer = smtpServer;
            Port = port;
            Password = password;
            From = @from;
            Caption = caption;
            MessageTemplate = messageTemplate;
            BasicEmailTimeout = TimeSpan.FromSeconds(basicEmailTimeoutInSecond);
            TimeoutIncrement = TimeSpan.FromSeconds(timeoutIncrementInSeconds);
        }

        public string SmtpServer { get; }
        public int Port { get; }
        public string Password { get; }

        public string From { get; }
        public string MessageTemplate { get; }
        public string Caption { get; }

        public TimeSpan BasicEmailTimeout { get; }
        public TimeSpan TimeoutIncrement { get; }

        public SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            client.Host = SmtpServer;
            client.Port = Port;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(From, Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}