namespace UserManagement.Application
{
    public interface IMailer
    {
        void SendConfirmationMail(string confirmationToken, string email);
    }
}