using Common;
using RabbitMQEventBus;

namespace Mailing
{
    public interface INotificationEmailDescriber
    {
        string Describe(IEventInfo @event);
    }
}