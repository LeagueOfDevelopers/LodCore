namespace LodCore.Infrastructure.Mailing.EmailModels
{
    public class PasswordRecovery
    {
        public PasswordRecovery(string userName, string passwordRecoveryLink)
        {
            UserName = userName;
            PasswordRecoveryLink = passwordRecoveryLink;
        }

        public string UserName { get; set; }
        public string PasswordRecoveryLink { get; set; }
    }
}
