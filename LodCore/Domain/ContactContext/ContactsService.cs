using ContactContext.Events;
using Journalist;
using Common;

namespace ContactContext
{
    public class ContactsService : IContactsService
    {
        public ContactsService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public void SendContactMessage(NewContactMessage contactMessage)
        {
            Require.NotNull(contactMessage, nameof(contactMessage));
            _eventPublisher.PublishEvent(contactMessage);
        }
		
        private readonly IEventPublisher _eventPublisher;
    }
}
