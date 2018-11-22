using LodCore.Domain.NotificationService;

namespace LodCore.Infrastructure.Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(string userName, IEventInfo @event);
    }
}