using EasyNetQ;
using EasyNetQ.Topology;

namespace RabbitMQEventBus
{
    public interface IEventBus
    {
        IAdvancedBus GetBusConnection();

        IMessage<T> WrapInMessage<T>(T @event)
            where T:class;

        IExchange GetExchange(string exchangeName);

        IQueue GetQueue(string queueName);

        void SetConsumer(string queueName, dynamic handlerFunction);
    }
}
