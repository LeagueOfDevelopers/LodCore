using ContactContext.Events;
using Journalist;
using NotificationService;
using RabbitMQEventBus;

namespace ContactContext
{
    public class ContactsService : IContactsService
    {
        public ContactsService(IEventSink contactsEventSink)
        {
            Require.NotNull(contactsEventSink, nameof(contactsEventSink));
            _contactsEventSink = contactsEventSink;
        }

        public void SendContactMessage(NewContactMessage contactMessage)
        {
            EventBus.GetBusConnection().Publish(
                EventBus.GetExchange("Notification"),
                "admin_notification_info",
                false,
                EventBus.WrapInMessage(contactMessage));
            Require.NotNull(contactMessage, nameof(contactMessage));
            _contactsEventSink.ConsumeEvent(contactMessage);
        }

        private readonly IEventSink _contactsEventSink;
    }
}
