using Common;

namespace NotificationService
{
    public interface IEventSink
    {
         void Consume(IEventInfo eventInfo);
    }
}