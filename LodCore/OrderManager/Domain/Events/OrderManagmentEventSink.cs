using Journalist;
using NotificationService;

namespace OrderManagement.Domain.Events
{
    internal class OrderManagmentEventSink : EventSinkBase
    {
        public OrderManagmentEventSink(IDistributionPolicyFactory distributionPolicyFactory,
            IEventRepository eventRepository, IEmailManager emailManager, IMailer mailer)
            : base(distributionPolicyFactory, eventRepository, emailManager, mailer)
        {
        }

        public override void ConsumeEvent(IEventInfo eventInfo)
        {
            Require.NotNull(eventInfo, nameof(eventInfo));

            var @event = new Event(eventInfo);

            var distributionPolicy = DistributionPolicyFactory.GetAdminRelatedPolicy();

            EventRepository.DistrubuteEvent(@event, distributionPolicy);

            ConfigureEmailByEvent(distributionPolicy.ReceiverIds, eventInfo);
        }
    }
}