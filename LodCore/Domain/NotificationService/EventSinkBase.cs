using System.Linq;
using Journalist;
using UserPresentaton;

namespace NotificationService
{
    public class EventSinkBase : IEventSink
    {
        public EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, 
            IEventRepository eventRepository,
            IMailer mailer, 
            IUserPresentationProvider userPresentationProvider)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));
            Require.NotNull(mailer, nameof(mailer));
            Require.NotNull(userPresentationProvider, nameof(userPresentationProvider));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            Mailer = mailer;
            _userPresentationProvider = userPresentationProvider;
        }

        protected IMailer Mailer { get; }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        private readonly IUserPresentationProvider _userPresentationProvider;

        public virtual void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            Mailer.SendNotificationEmail(distributionPolicy.ReceiverIds, eventInfo);
        }

        protected void SendOutEmailsAboutEvent(int[] userIds, IEventInfo eventInfo)
        {
            var ids =
                userIds.Where(
                    id =>
                        _userPresentationProvider.GetUserEventSettings(id, eventInfo.GetEventType()) ==
                        NotificationSettingValue.SendNotificationAndMail).ToArray();

            Mailer.SendNotificationEmail(ids, eventInfo);
        }
    }
}