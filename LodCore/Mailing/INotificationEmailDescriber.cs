using NotificationService;

namespace Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(IEventInfo @event);
    }
}