using Journalist;
using LodCore.Domain.NotificationService;
using LodCore.Infrastructure.EventBus;

namespace LodCore.Infrastructure.ContactContext
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
