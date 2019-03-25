namespace LodCoreLibraryOld.Infrastructure.EventBus
{
    public interface IEventPublisherProvider
    {
        IEventPublisher GetEventPublisher();
    }
}