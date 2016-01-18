namespace Mailing
{
    public class MailerSettings
    {
        public MailerSettings( string smtpServer= "smtp.yandex.ru", int port = 25, string password= "q4l7w1kzjVO9PErK5dkR",
            string @from= @"mail@lod-misis.ru", string captionForConfirmation= "Подтверждение аккаунта на сайте Лиги Разработчиков",
            string captionForNotification = "В Лиге Разработчиков произошло кое-что интересное!")
        {
            CaptionForNotification = captionForNotification;
            SmtpServer = smtpServer;
            Port = port;
            Password = password;
            From = @from;
            CaptionForConfirmation = captionForConfirmation;
            ConfirmationMessageTemplate = ValidationMessageResources.ConfirmationMessageTemplate;
            NotificationMessageTemplate = ValidationMessageResources.NotificationMessageTemplate;

        }

        public string SmtpServer { get; }
        public int Port { get; }
        public string Password { get; }

        public string From { get; }

        public string ConfirmationMessageTemplate { get; }
        public string CaptionForConfirmation { get; }// = "Подтверждение аккаунта на сайте Лиги Разработчиков";

        public string NotificationMessageTemplate { get; }
        public string CaptionForNotification { get; }// = "Подтверждение аккаунта на сайте Лиги Разработчиков";

        /*

        */
    }
}