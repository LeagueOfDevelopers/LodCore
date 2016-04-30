using System.Net.Mail;

namespace UserManagement.Application
{
    public interface IMailer
    {
        void SendConfirmationMail(string confirmationLink, MailAddress emailAddress);

        void SendPasswordResertMail(string resetLink, MailAddress emailAddress);
    }
}