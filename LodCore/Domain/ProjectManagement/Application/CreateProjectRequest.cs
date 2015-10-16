using Journalist;
using ProjectManagement.Domain;

namespace ProjectManagement.Application
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest(
            string name, 
            ProjectType projectType, 
            string info, 
            AccessLevel accessLevel)
        {
            Require.NotEmpty(name, nameof(name));
            Require.NotEmpty(info, nameof(info));

            Name = name;
            ProjectType = projectType;
            Info = info;
            AccessLevel = accessLevel;
        }

        public string Name { get; private set; }

        public ProjectType ProjectType { get; private set; }

        public string Info { get; private set; }

        public AccessLevel AccessLevel { get; private set; }
    }
}