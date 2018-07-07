using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreLibrary.Infrastructure.EventBus
{
    public interface IEventPublisher
    {
        void PublishEvent<T>(T @event) where T : EventInfoBase;
    }
}
