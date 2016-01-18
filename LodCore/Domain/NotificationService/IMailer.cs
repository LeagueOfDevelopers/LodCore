using System.Net.Mail;

namespace NotificationService
{
    public interface IMailer
    {
        void SendNotificationEmail(MailAddress emailAddress, IEventInfo eventInfo);
    }
}