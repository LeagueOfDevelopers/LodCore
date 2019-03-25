using System.Net.Mail;
using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.Mailing
{
    public interface IMailer
    {
        void SendNotificationEmail(int[] userIds, IEventInfo eventInfo);

        void SendConfirmationMail(string userName, string confirmationLink, MailAddress emailAddress);

        void SendPasswordResetMail(string userName, string resetLink, MailAddress emailAddress);
    }
}