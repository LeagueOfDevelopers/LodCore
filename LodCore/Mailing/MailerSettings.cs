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

        public string SmtpServer { get; }// = "217.69.139.160";
        public int Port { get; }// = 465;
        public string Password { get; }// = "F0rjustice";

        public string From { get; }// = @"leagueofdevelopers@mail.ru";
        public string MessageTemplate { get; }// = ValidationMessageResources.messageTemplate;
        public string Caption { get; }// = "Подтверждение аккаунта на сайте Лиги Разработчиков";
    }
}