using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class NewDeveloperOnProject : Event
    {
        public NewDeveloperOnProject(int userId, int projectId, DistributionPolicy distributionPolicy) : base(distributionPolicy)
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