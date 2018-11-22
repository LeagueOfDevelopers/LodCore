using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(string userName, IEventInfo @event);
    }
}