using Common;
using EasyNetQ;
using EasyNetQ.Topology;

namespace RabbitMQEventBus
{
	public class EventConsumersContainer : IEventConsumersContainer
	{
		public EventConsumersContainer(EventBusSettings eventBusSettings)
		{
			_eventBusSettings = eventBusSettings;
			_bus = InitializeBusConnection();
			_mainExchange = _bus.ExchangeDeclare(mainExchangeName, ExchangeType.Direct);
		}

		public void RegisterConsumer<T>(IEventConsumer<T> consumer) where T : EventInfoBase
		{
			var queue = _bus.QueueDeclare(GetQueueNameForConsumer(consumer));
			var routingKey = GetRoutingKeyForEvent<T>();
			_bus.Bind(_mainExchange, queue, routingKey);
			_bus.Consume<T>(queue, (message, info) => consumer.Consume(message.Body));
		}

		public IEventPublisher GetEventPublisher()
		{
			return new EventPublisher(_bus, _mainExchange);	
		}

		public void StartListening()
		{
			
		}

		public void StopListening()
		{
            _bus.Dispose();
		}

		private static string GetQueueNameForConsumer<T>(IEventConsumer<T> consumer)
		{
			return $"{typeof(T).FullName}-{consumer.GetType().FullName}";
		}

		private static string GetRoutingKeyForEvent<T>() where T : EventInfoBase
		{
			return typeof(T).FullName;
		}

		private IAdvancedBus InitializeBusConnection()
		{
			var connectionString = $"host={_eventBusSettings.HostName}; " +
			                       $"virtualHost={_eventBusSettings.VirtualHost}; " +
			                       $"username={_eventBusSettings.UserName}; " +
			                       $"password={_eventBusSettings.Password}";
			var bus = RabbitHutch.CreateBus(connectionString).Advanced;
			return bus;
		}

		private readonly EventBusSettings _eventBusSettings;
		private readonly IAdvancedBus _bus;
		private readonly IExchange _mainExchange;
		const string mainExchangeName = "all-events";
	}
}