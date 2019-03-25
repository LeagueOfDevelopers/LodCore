namespace LodCore.Infrastructure.Mailing.EmailModels
{
    public class NewProject
    {
        public NewProject(
            string userName,
            string projectName,
            string projectDescription)
        {
            UserName = userName;
            ProjectName = projectName;
            ProjectDescription = projectDescription;
        }

        public string UserName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}