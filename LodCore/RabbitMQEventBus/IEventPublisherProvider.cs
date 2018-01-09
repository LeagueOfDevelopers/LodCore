using Common;

namespace RabbitMQEventBus
{
    public interface IEventPublisherProvider
    {
        IEventPublisher GetEventPublisher();
    }
}
