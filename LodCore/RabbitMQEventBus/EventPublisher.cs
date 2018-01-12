using Common;
using EasyNetQ;
using EasyNetQ.Topology;

namespace RabbitMQEventBus
{
	public class EventPublisher : IEventPublisher
	{
		public EventPublisher(IAdvancedBus bus, IExchange exchange)
		{
			_bus = bus;
			_exchange = exchange;
		}

		public void PublishEvent<T>(T @event) where T : EventInfoBase
		{
			_bus.Publish(_exchange, typeof(T).FullName, false, new Message<T>(@event));
		}

		private readonly IAdvancedBus _bus;
		private readonly IExchange _exchange;
	}
}