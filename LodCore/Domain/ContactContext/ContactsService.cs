using ContactContext.Events;
using Journalist;
using NotificationService;
using RabbitMQEventBus;

namespace ContactContext
{
    public class ContactsService : IContactsService
    {
        public ContactsService(IEventSink contactsEventSink, IEventBus eventBus)
        {
            Require.NotNull(contactsEventSink, nameof(contactsEventSink));
            _contactsEventSink = contactsEventSink;
            _eventBus = eventBus;
        }

        public void SendContactMessage(NewContactMessage contactMessage)
        {
            Require.NotNull(contactMessage, nameof(contactMessage));
            _eventBus.PublishEvent("Notification", "new_contact_message", contactMessage);
        }

        private readonly IEventSink _contactsEventSink;
        private readonly IEventBus _eventBus;
    }
}
