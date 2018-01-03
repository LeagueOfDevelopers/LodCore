using Common;
using RabbitMQEventBus;

namespace NotificationService
{
    public interface IEventSink
    {
         void ConsumeEvent(IEventInfo eventInfo);
    }
}