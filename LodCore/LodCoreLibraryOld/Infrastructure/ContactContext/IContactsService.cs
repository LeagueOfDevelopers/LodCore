using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.ContactContext
{
    public interface IContactsService
    {
        void SendContactMessage(NewContactMessage contactMessage);
    }
}