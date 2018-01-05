using Common;
using Journalist;
using UserManagement.Domain.Events;
using UserPresentaton;

namespace NotificationService
{
    public class UserManagementEventSink : EventSinkBase
    {
        public UserManagementEventSink(IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository, 
            IMailer mailer, 
            IUserPresentationProvider userPresentationProvider)
            : base(distributionPolicyFactory, 
                  eventRepository, 
                  mailer, 
                  userPresentationProvider)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = GetDistributionPolicyForEvent((dynamic)eventInfo);

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