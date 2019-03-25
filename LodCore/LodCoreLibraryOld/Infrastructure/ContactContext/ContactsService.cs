using Journalist;
using LodCoreLibraryOld.Domain.NotificationService;
using LodCoreLibraryOld.Infrastructure.EventBus;

namespace LodCoreLibraryOld.Infrastructure.ContactContext
{
    public class ContactsService : IContactsService
    {
        private readonly IEventPublisher _eventPublisher;

        public ContactsService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public void SendContactMessage(NewContactMessage contactMessage)
        {
            Require.NotNull(contactMessage, nameof(contactMessage));
            _eventPublisher.PublishEvent(contactMessage);
        }
    }
}