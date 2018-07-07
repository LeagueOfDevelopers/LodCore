using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreLibrary.Infrastructure.ContactContext
{
    public interface IContactsService
    {
        void SendContactMessage(NewContactMessage contactMessage);
    }
}