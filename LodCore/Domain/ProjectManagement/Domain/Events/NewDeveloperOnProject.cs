using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class NewDeveloperOnProject : EventInfoBase
    {
        public NewDeveloperOnProject(int userId, int projectId)
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