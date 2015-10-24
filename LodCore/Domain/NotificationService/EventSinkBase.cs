using Journalist;

namespace NotificationService
{
    public abstract class EventSinkBase : IEventSink
    {
        protected EventSinkBase(IDistributionPolicyFactory distributionPolicyFactory, IEventRepository eventRepository)
        {
            Require.NotNull(distributionPolicyFactory, nameof(distributionPolicyFactory));
            Require.NotNull(eventRepository, nameof(eventRepository));

            DistributionPolicyFactory = distributionPolicyFactory;
            EventRepository = eventRepository;
        }

        public abstract void ConsumeEvent(IEventInfo eventInfo);

        protected IDistributionPolicyFactory DistributionPolicyFactory { get; private set; }

        protected IEventRepository EventRepository { get; private set; }
        
    }
}