namespace UserManagement.Application
{
    internal interface IMailer
    {
        void SendConfirmationMail(string confirmationToken, string email);
    }
}