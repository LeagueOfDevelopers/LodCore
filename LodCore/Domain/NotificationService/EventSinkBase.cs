using Journalist;

namespace NotificationService
{
    public abstract class EventSinkBase : IEventSink
    {
        protected EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository,
            IMailer mailer)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));
            Require.NotNull(mailer, nameof(mailer));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            Mailer = mailer;
        }

        protected IMailer Mailer { get; }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        public abstract void ConsumeEvent(IEventInfo eventInfo);

        protected void SendOutEmailsAboutEvent(int[] userIds, IEventInfo eventInfo)
        {
            Mailer.SendNotificationEmail(userIds, eventInfo);
        }
    }
}