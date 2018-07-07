using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreLibrary.Infrastructure.Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(string userName, IEventInfo @event);
    }
}