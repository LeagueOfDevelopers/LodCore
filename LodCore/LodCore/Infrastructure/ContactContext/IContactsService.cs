using LodCore.Domain.NotificationService;

namespace LodCore.Infrastructure.ContactContext
{
    public interface IContactsService
    {
        void SendContactMessage(NewContactMessage contactMessage);
    }
}