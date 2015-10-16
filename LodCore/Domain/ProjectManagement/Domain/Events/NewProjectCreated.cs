using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class NewProjectCreated : Event
    {
        public NewProjectCreated(int projectId, DistributionPolicy distributionPolicy) : base(distributionPolicy)
        {
            Require.Positive(projectId, nameof(projectId));

            ProjectId = projectId;
        }

        public int ProjectId { get; private set; }
    }
}