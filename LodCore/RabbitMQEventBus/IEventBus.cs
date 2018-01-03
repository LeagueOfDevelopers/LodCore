using Common;

namespace RabbitMQEventBus
{
    public interface IEventBus
    {
        void PublishEvent<T>(T @event) where T : EventInfoBase;

        void SetConsumer(string queueName, dynamic handlerFunction);
    }
}
