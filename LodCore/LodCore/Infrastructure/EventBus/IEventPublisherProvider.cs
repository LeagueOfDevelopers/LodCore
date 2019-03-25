namespace LodCore.Infrastructure.EventBus
{
    public interface IEventPublisherProvider
    {
        IEventPublisher GetEventPublisher();
    }
}