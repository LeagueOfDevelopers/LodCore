using Common;
using Journalist;

namespace ProjectManagement.Domain.Events
{
    public class NewDeveloperOnProject : EventInfoBase
    {
        public NewDeveloperOnProject(int userId, int projectId, 
            string firstName, string lastName, string projectName)
        {
            Require.Positive(userId, nameof(userId));
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));
            Require.NotEmpty(projectName, nameof(projectName));

            UserId = userId;
            ProjectId = projectId;
            FirstName = firstName;
            LastName = lastName;
            ProjectName = projectName;
        }

        public int UserId { get; private set; }
        public int ProjectId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string ProjectName { get; private set; }
    }
}