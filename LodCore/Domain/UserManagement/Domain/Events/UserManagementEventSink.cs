using Journalist;
using NotificationService;

namespace UserManagement.Domain.Events
{
    public class UserManagementEventSink : EventSinkBase
    {
        public UserManagementEventSink(IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository, IMailer mailer)
            : base(distributionPolicyFactory, eventRepository, mailer)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = GetDistributionPolicyForEvent((dynamic) eventInfo);

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            SendOutEmailsAboutEvent(distributionPolicy.ReceiverIds, eventInfo);
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewEmailConfirmedDeveloper eventInfo)
        {
            return DistributionPolicyFactory.GetAdminRelatedPolicy();
        }

        private DistributionPolicy GetDistributionPolicyForEvent(NewFullConfirmedDeveloper eventInfo)
        {
            return DistributionPolicyFactory.GetAllPolicy();
        }
    }
}