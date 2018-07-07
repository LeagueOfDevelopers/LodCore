using LodCoreLibrary.Domain.NotificationService;

namespace NotificationService
{
    public interface IEventSink
    {
         void Consume(IEventInfo eventInfo);
    }
}