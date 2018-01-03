using Common;

namespace RabbitMQEventBus
{
	public interface IEventPublisher
	{
		void PublishEvent<T>(T @event) where T : EventInfoBase;
	}
}