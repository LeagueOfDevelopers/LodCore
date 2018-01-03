namespace RabbitMQEventBus
{
	public interface IEventConsumer<in T>
	{
		void Consume(T @event);
	}
}