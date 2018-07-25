using EasyNetQ;
using EasyNetQ.Topology;
using System.Threading.Tasks;
using Serilog;
using System;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreLibrary.Infrastructure.EventBus
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
                    try
                    {
                        _databaseSessionProvider.OpenSession();
                        consumer.Consume(message.Body);
                        _databaseSessionProvider.CloseSession();
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Failed to consume message with info={@0}: {1} StackTrace: {2}",
                    info, ex.Message, ex.StackTrace);
                    }
                    Log.Information("Message {@0} has consumed", info);
                }
            ));
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
            Log.Information("Event bus listening has stopped");
		}

		private static string GetQueueNameForConsumer<T>(IEventConsumer<T> consumer) 
            where T : EventInfoBase
		{
			return $"{typeof(T).FullName}";
		}

		private static string GetRoutingKeyForEvent<T>() 
            where T : EventInfoBase
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
            Log.Information("Event bus listening has started");
        }

        private readonly IDatabaseSessionProvider _databaseSessionProvider;
		private readonly EventBusSettings _eventBusSettings;
		private IAdvancedBus _bus;
		private IExchange _mainExchange;
		const string mainExchangeName = "all-events";
	}
}