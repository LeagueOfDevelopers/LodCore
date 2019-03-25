namespace LodCoreLibraryOld.Infrastructure.Mailing.EmailModels
{
    public class EmailConfirmation
    {
        public EmailConfirmation(string userName, string confirmationLink)
        {
            UserName = userName;
            ConfirmationLink = confirmationLink;
        }

        public string UserName { get; set; }
        public string ConfirmationLink { get; set; }
    }
}