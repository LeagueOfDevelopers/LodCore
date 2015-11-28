namespace Mailing
{
    public class MailerSettings
    {
        public string SmtpServer { get; } = "217.69.139.160";
        public int Port { get; } = 465;
        public string Password { get; } = "F0rjustice";

        public string From { get; } = @"leagueofdevelopers@mail.ru";
        public string MessageTemplate { get; } = ValidationMessageResources.messageTemplate;
        public string Caption { get; } = "Подтверждение аккаунта на сайте Лиги Разработчиков";
    }
}