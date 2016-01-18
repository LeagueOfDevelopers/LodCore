using Journalist;

namespace NotificationService
{
    public abstract class EventSinkBase : IEventSink
    {
        protected EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository,
            IEmailManager emailManager,
            IMailer mailer)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));
            Require.NotNull(emailManager, nameof(emailManager));
            Require.NotNull(mailer, nameof(mailer));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            EmailManager = emailManager;
            Mailer = mailer;
        }

        protected IEmailManager EmailManager { get; }

        protected IMailer Mailer { get; }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        public abstract void ConsumeEvent(IEventInfo eventInfo);

        protected void SendOutEmailsAboutEvent(int[] userIds, IEventInfo eventInfo)
        {
            foreach (var userId in userIds)
            {
                var currentMailAdress = EmailManager.GetMailAddressById(userId);
                Mailer.SendNotificationEmail(currentMailAdress, eventInfo);
            }
        }
    }
}