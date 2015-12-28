using System.Net.Mail;

namespace NotificationService.Application
{
    public interface IEmailManager
    {
        MailAddress GetEmailById(int userId);
    }
}