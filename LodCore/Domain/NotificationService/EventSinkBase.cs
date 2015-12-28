using System.Linq;
using Journalist;
using NotificationService.Application;

namespace NotificationService
{
    public abstract class EventSinkBase : IEventSink
    {
        protected EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository,
            IMailer mailer, IEmailManager emailManager)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
            Mailer = mailer;
            EmailManager = emailManager;
        }

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }

        protected IMailer Mailer { get; private set; }
        protected IEmailManager EmailManager { get; private set; }

        public abstract void ConsumeEvent(IEventInfo eventInfo);

        protected void ConfigureEmailByEvent(IEventInfo eventInfo, int[] reciverIds)
        {
            var emailArray = reciverIds.Select(id => EmailManager.GetEmailById(id));
            Mailer.ConfigureEmailByEventForEmails(emailArray.ToArray(), eventInfo);
        }
    }
}