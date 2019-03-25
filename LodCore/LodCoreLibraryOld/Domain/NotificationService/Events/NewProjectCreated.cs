using Journalist;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class NewProjectCreated : EventInfoBase
    {
        public NewProjectCreated(int projectId, string projectName)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(projectName, nameof(projectName));

            ProjectId = projectId;
            ProjectName = projectName;
        }

        public int ProjectId { get; }
        public string ProjectName { get; }
    }
}