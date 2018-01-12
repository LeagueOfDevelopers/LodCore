using Common;
using Journalist;

namespace ProjectManagement.Domain.Events
{
    public class NewProjectCreated : EventInfoBase
    {
        public NewProjectCreated(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            ProjectId = projectId;
        }

        public int ProjectId { get; private set; }
    }
}