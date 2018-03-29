using Common;
using Journalist;

namespace ProjectManagement.Domain.Events
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

        public int ProjectId { get; private set; }
        public string ProjectName { get; private set; }
    }
}