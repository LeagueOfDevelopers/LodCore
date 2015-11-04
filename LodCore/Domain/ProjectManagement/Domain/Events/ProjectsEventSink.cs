using Journalist;
using NotificationService;

namespace ProjectManagement.Domain.Events
{
    public class ProjectsEventSink : EventSinkBase
    {
        public ProjectsEventSink(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository)
            : base(distributionPolicyFactory, eventRepository)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = GetDistributionPolicyForEvent((dynamic) eventInfo);

            EventRepository.DistrubuteEvent(@event, distributionPolicy);
        }

        private DistributionPolicy GetDistributionPolicyForEvent(DeveloperHasLeftProject @eventInfo)
        {
            return
                DistributionPolicyFactory.GetProjectRelatedPolicy(@eventInfo.ProjectId)
                    .Merge(DistributionPolicyFactory.GetAdminRelatedPolicy());
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewDeveloperOnProject @eventInfo)
        {
            return
                DistributionPolicyFactory.GetProjectRelatedPolicy(@eventInfo.ProjectId)
                    .Merge(DistributionPolicyFactory.GetAdminRelatedPolicy());
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewProjectCreated @eventInfo)
        {
            return DistributionPolicyFactory.GetAllPolicy();
        }
    }
}