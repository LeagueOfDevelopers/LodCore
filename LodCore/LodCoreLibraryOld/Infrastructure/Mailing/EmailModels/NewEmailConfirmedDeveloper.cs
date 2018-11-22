namespace LodCoreLibraryOld.Infrastructure.Mailing.EmailModels
{
    public class NewEmailConfirmedDeveloper
    {
        public NewEmailConfirmedDeveloper(
            string userName, 
            string developerName,
            string developerEmail)
        {
            UserName = userName;
            DeveloperName = developerName;
            DeveloperEmail = developerEmail;
        }

        public string UserName { get; set; }
        public string DeveloperName { get; set; }
        public string DeveloperEmail { get; set; }
    }
}
