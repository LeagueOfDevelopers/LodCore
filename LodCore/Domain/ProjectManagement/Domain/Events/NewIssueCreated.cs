using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class NewIssueCreated : Event
    {
        public NewIssueCreated(Issue issue, int projectId, DistributionPolicy distributionPolicy) : base(distributionPolicy)
        {
            Require.NotNull(issue, nameof(issue));
            Require.Positive(projectId, nameof(projectId));

            Issue = issue;
            ProjectId = projectId;
        }

        public Issue Issue { get; private set; }

        public int ProjectId { get; private set; }
    }
}