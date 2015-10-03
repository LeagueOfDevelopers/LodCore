using System.Linq;
using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class ProjectBoundedDistributionPolicy : IDistributionPolicy
    {
        public ProjectBoundedDistributionPolicy(Project project)
        {
            Require.NotNull(project, nameof(project));

            ReceiversIds = project.ProjectUserIds.ToArray();
        }

        public int[] ReceiversIds { get; }
    }
}