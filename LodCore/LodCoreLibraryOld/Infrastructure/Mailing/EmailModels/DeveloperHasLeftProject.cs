namespace LodCoreLibraryOld.Infrastructure.Mailing.EmailModels
{
    public class DeveloperHasLeftProject
    {
        public DeveloperHasLeftProject(
            string userName,
            string developerName,
            string projectName)
        {
            UserName = userName;
            DeveloperName = developerName;
            ProjectName = projectName;
        }

        public string UserName { get; set; }
        public string DeveloperName { get; set; }
        public string ProjectName { get; set; }
    }
}
