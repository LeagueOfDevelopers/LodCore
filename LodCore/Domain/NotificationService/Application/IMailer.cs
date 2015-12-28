using System.Net.Mail;

namespace NotificationService.Application
{
    public interface IMailer
    {
        void ConfigureEmailByEventForEmail(MailAddress email, IEventInfo eventInfo);
        void ConfigureEmailByEventForEmails(MailAddress[] emailArray, IEventInfo eventInfo);

    }
}