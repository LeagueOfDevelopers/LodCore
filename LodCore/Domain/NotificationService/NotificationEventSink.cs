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

            var distributionPolicy = DistributionPolicyFactory.GetAllPolicy();

            EventRepository.SaveEvent(new Event(eventInfo), distributionPolicy);

            SendOutEmailsAboutEvent(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}