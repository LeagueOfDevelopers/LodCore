
namespace Mailing.EmailModels
{
    public class NewFullConfirmedDeveloper
    {
        public NewFullConfirmedDeveloper(string userName, string developerName)
        {
            UserName = userName;
            DeveloperName = developerName;
        }

        public string UserName { get; set; }
        public string DeveloperName { get; set; }
    }
}
