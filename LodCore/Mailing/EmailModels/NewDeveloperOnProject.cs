﻿
namespace Mailing.EmailModels
{
    public class NewDeveloperOnProject
    {
        public NewDeveloperOnProject(
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
