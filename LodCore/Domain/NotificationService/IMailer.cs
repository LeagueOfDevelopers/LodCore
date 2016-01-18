using System.Net.Mail;

namespace NotificationService
{
    public interface IMailer
    {
        void ConsumeNotificationEmail(MailAddress emailAddress, IEventInfo eventInfo);
    }
}