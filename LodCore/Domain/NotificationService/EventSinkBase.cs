using System.Linq;
using Journalist;
using UserPresentaton;

namespace NotificationService
{
    public abstract class EventSinkBase : IEventSink
    {
        private readonly IUserPresentationProvider _userPresentationProvider;

        protected EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository,
            IMailer mailer, IUserPresentationProvider userPresentationProvider)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));
            Require.NotNull(mailer, nameof(mailer));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            Mailer = mailer;
            _userPresentationProvider = userPresentationProvider;
        }

        protected IMailer Mailer { get; }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        public abstract void ConsumeEvent(IEventInfo eventInfo);

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