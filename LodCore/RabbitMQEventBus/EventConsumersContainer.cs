using Common;
using EasyNetQ;
using EasyNetQ.Topology;
using System.Threading.Tasks;

namespace RabbitMQEventBus
{
	public class EventConsumersContainer : IEventConsumersContainer, IEventPublisherProvider
	{
		public EventConsumersContainer(EventBusSettings eventBusSettings,
                                       IDatabaseSessionProvider databaseSessionProvider)
		{
			_eventBusSettings = eventBusSettings;
            _databaseSessionProvider = databaseSessionProvider;
		}

        public void RegisterConsumer<T>(IEventConsumer<T> consumer) where T : EventInfoBase
        {
            var queue = _bus.QueueDeclare(GetQueueNameForConsumer(consumer));
            var routingKey = GetRoutingKeyForEvent<T>();
            _bus.Bind(_mainExchange, queue, routingKey);
            _bus.Consume<T>(queue, (message, info) =>
            Task.Factory.StartNew(() =>
                {
                    _databaseSessionProvider.OpenSession();
                    consumer.Consume(message.Body);
                    _databaseSessionProvider.CloseSession();
                })
            );
		}

		public IEventPublisher GetEventPublisher()
		{
			return new EventPublisher(_bus, _mainExchange);	
		}

		public void StartListening()
		{
            InitializeBusConnection();
		}

		public void StopListening()
		{
            _bus.SafeDispose();
		}

		private static string GetQueueNameForConsumer<T>(IEventConsumer<T> consumer)
		{
			return $"{typeof(T).FullName}-{consumer.GetType().FullName}";
		}

		private static string GetRoutingKeyForEvent<T>() where T : EventInfoBase
		{
			return typeof(T).FullName;
		}

		private void InitializeBusConnection()
		{
            var connectionString = $"host={_eventBusSettings.HostName}; " +
			                       $"virtualHost={_eventBusSettings.VirtualHost}; " +
			                       $"username={_eventBusSettings.UserName}; " +
			                       $"password={_eventBusSettings.Password}";
			_bus = RabbitHutch.CreateBus(connectionString).Advanced;
            _mainExchange = _bus.ExchangeDeclare(mainExchangeName, ExchangeType.Direct);
        }

        private void StartConsumption(string queue) { 
}

        private readonly IDatabaseSessionProvider _databaseSessionProvider;
		private readonly EventBusSettings _eventBusSettings;
		private IAdvancedBus _bus;
		private IExchange _mainExchange;
		const string mainExchangeName = "all-events";
	}
}