using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class ProjectsEventSink : EventSink
    {
        public ProjectsEventSink(IEventRepository repository, IDistributionPolicyFactory distributionPolicyFactory) 
            : base(repository, distributionPolicyFactory)
        {
        }

        public void SendNewDeveloperEvent(int projectId, int userId)
        {
            ConsumeEvent(new NewDeveloperOnProject(
                userId, 
                projectId, 
                DistributionPolicyFactory.GetProjectRelatedPolicy(projectId)));
        }

        public void SendNewProjectCreatedEvent(int projectId)
        {
            ConsumeEvent(new NewProjectCreated(projectId, DistributionPolicyFactory.GetAllPolicy()));
        }

        public void SendDeveloperHasLeftProjectEvent(int projectId, int userId)
        {
            ConsumeEvent(new DeveloperHasLeftProject(
                userId, 
                projectId, 
                DistributionPolicyFactory.GetProjectRelatedPolicy(projectId)));
        }
    }
}