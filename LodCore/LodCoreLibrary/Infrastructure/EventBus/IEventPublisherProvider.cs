namespace LodCoreLibrary.Infrastructure.EventBus
{
    public interface IEventPublisherProvider
    {
        IEventPublisher GetEventPublisher();
    }
}
