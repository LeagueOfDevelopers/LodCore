using Journalist;
using NotificationService;
using UserPresentaton;

namespace ContactContext.Events
{
    public class ContactsEventSink : EventSinkBase
    {
        public ContactsEventSink(
            IDistributionPolicyFactory distributionPolicyFactory, 
            IEventRepository eventRepository, 
            IMailer mailer, IUserPresentationProvider userPresentationProvider) : base(distributionPolicyFactory, eventRepository, mailer, userPresentationProvider)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            Mailer.SendNotificationEmail(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}