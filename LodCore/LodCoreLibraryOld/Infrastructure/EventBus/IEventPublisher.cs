using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreLibraryOld.Infrastructure.EventBus
{
    public interface IEventPublisher
    {
        void PublishEvent<T>(T @event) where T : EventInfoBase;
    }
}
