using Journalist;

namespace LodCoreLibraryOld.Domain.NotificationService
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

        public int UserId { get; }
        public int ProjectId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string ProjectName { get; }
    }
}