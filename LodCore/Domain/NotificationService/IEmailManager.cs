using System.Net.Mail;

namespace NotificationService
{
    public interface IEmailManager
    {
        MailAddress GetMailAddressById(int userId);
    }
}