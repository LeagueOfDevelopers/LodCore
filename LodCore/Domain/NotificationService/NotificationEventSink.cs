using Common;
using Journalist;
using UserPresentaton;

namespace NotificationService
{
    public class NotificationEventSink<T> : EventSinkBase<T> where T : IEventInfo
    {
        public NotificationEventSink(IDistributionPolicyFactory distributionPolicyFactory, 
                                     IEventRepository eventRepository, 
                                     IMailer mailer, 
                                     IUserPresentationProvider userPresentationProvider) : 
                                base(distributionPolicyFactory, 
                                     eventRepository, 
                                     mailer, 
                                     userPresentationProvider)
        {   
        }

        public override void Consume(T eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAllPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            SendOutEmailsAboutEvent(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}