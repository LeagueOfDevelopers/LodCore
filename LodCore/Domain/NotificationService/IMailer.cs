using Common;
using System.Net.Mail;

namespace NotificationService
{
    public interface IMailer
    {
        void SendNotificationEmail(int[] userIds, IEventInfo eventInfo);

        void SendConfirmationMail(string confirmationLink, MailAddress emailAddress);

        void SendPasswordResetMail(string resetLink, MailAddress emailAddress);
    }
}