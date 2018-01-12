using Common;
using Journalist;

namespace ProjectManagement.Domain.Events
{
    public class DeveloperHasLeftProject : EventInfoBase
    {
        public DeveloperHasLeftProject(int userId, int projectId)
        {
            Require.Positive(userId, nameof(userId));
            Require.Positive(projectId, nameof(projectId));

            UserId = userId;
            ProjectId = projectId;
        }

        public int UserId { get; private set; }

        public int ProjectId { get; private set; }
    }
}