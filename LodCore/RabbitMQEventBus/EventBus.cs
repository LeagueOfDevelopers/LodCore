using EasyNetQ;
using Journalist;

namespace RabbitMQEventBus
{
    public class EventBus
    {
        public EventBus(EventBusSettings eventBusSettings)
        {
            Require.NotNull(eventBusSettings, nameof(eventBusSettings));
            _eventBusSettings = eventBusSettings;
            Bus = InitializeBusConnection();
            DeclareExchanges();
            DeclareQueues();
            DeclareBindings();
        }

        public IAdvancedBus InitializeBusConnection()
        {
            /*var connectionString = $"host = {_eventBusSettings.HostName}; " +
                $"virtualHost = {_eventBusSettings.VirtualHost}; " +
                $"username = {_eventBusSettings.UserName}; " +
                $"password = {_eventBusSettings.Password}";*/
            var connectionString = "host=localhost";
            var bus = RabbitHutch.CreateBus(connectionString).Advanced;
            return bus;
        }

        private void DeclareExchanges()
        {

        }

        private void DeclareQueues()
        {

        }

        private void DeclareBindings()
        {

        }

        private readonly EventBusSettings _eventBusSettings;
        public IAdvancedBus Bus { get; }
    }
}
