using LodCore.Domain.NotificationService;

namespace LodCore.Infrastructure.EventBus
{
    public interface IEventPublisher
    {
        void PublishEvent<T>(T @event) where T : EventInfoBase;
    }
}
