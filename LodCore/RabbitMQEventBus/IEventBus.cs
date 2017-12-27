namespace RabbitMQEventBus
{
    public interface IEventBus
    {
        void PublishEvent<T>(string exchangeName, string routeName, T @event)
            where T : class;

        void SetConsumer(string queueName, dynamic handlerFunction);
    }
}
