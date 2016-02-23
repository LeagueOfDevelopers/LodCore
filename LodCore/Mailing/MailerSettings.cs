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
            string messageTemplate)
        {
            Require.NotEmpty(smtpServer, nameof(smtpServer));
            Require.Positive(port, nameof(port));
            Require.NotEmpty(password, nameof(password));
            Require.NotEmpty(@from, nameof(@from));
            Require.NotEmpty(caption, nameof(caption));
            Require.NotEmpty(messageTemplate, nameof(messageTemplate));

            SmtpServer = smtpServer;
            Port = port;
            Password = password;
            From = @from;
            Caption = caption;
            MessageTemplate = messageTemplate;
        }

        public string SmtpServer { get; }
        public int Port { get; }
        public string Password { get; }

        public string From { get; }
        public string MessageTemplate { get; }
        public string Caption { get; }
    }
}