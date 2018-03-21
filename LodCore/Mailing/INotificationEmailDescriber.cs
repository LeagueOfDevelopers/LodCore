using Common;

namespace Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(string userName, IEventInfo @event);
    }
}