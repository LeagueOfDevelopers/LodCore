using EasyNetQ;
using EasyNetQ.Topology;
using Journalist;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace RabbitMQEventBus
{
    public class EventBus : IEventBus
    {
        public EventBus(EventBusSettings eventBusSettings)
        {
            Require.NotNull(eventBusSettings, nameof(eventBusSettings));
            _eventBusSettings = eventBusSettings;
            _bus = InitializeBusConnection();
            DeclareExchanges();
            DeclareQueues();
            DeclareBindings();
        }

        public void PublishEvent<T>(string exchangeName, string routeName, T @event)
            where T:class
        {
            _bus.Publish(
                GetExchange(exchangeName),
                routeName,
                false,
                WrapInMessage(@event));
        }

	    public void PublishEvent<T>(T @event) where T : EventInfoBase
	    {
		    throw new System.NotImplementedException();
	    }

	    public void SetConsumer(string queueName, dynamic handlerFunction)
        {
            _bus.Consume<dynamic>(GetQueue(queueName),
                (msg, info) => Task.Factory.StartNew(() =>
                {
                    handlerFunction.DynamicInvoke(msg.Body);
                }));
        }

        private IMessage<T> WrapInMessage<T>(T @event)
            where T : class
        {
            return new Message<T>(@event);
        }

        private IExchange GetExchange(string exchangeName)
        {
            if (_exchanges.ContainsKey(exchangeName))
                return _exchanges[exchangeName];
            else
                throw new KeyNotFoundException("Exchange doesn't exist");
        }

        private IQueue GetQueue(string queueName)
        {
            if (_queues.ContainsKey(queueName))
                return _queues[queueName];
            else
                throw new KeyNotFoundException("Queue doesn't exist");
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

        private void DeclareExchanges()
        {
            _exchanges = new Dictionary<string, IExchange>();
            _exchanges.Add("MailValidationRequest", _bus.ExchangeDeclare(
                           "mail_validation_request_events", ExchangeType.Direct));
            _exchanges.Add("Notification", _bus.ExchangeDeclare(
                           "notification_events", ExchangeType.Direct));
            _exchanges.Add("NewContactMessage", _bus.ExchangeDeclare(
                "new_contact_message_events", ExchangeType.Fanout));
            _exchanges.Add("DeveloperHasLeftProject", _bus.ExchangeDeclare(
                           "developer_has_left_project_events", ExchangeType.Fanout));
            _exchanges.Add("NewDeveloperOnProject", _bus.ExchangeDeclare(
                           "new_developer_on_project_events", ExchangeType.Fanout));
            _exchanges.Add("NewEmailConfirmedDeveloper", _bus.ExchangeDeclare(
                           "new_email_confirmed_developer_events", ExchangeType.Fanout));
            _exchanges.Add("NewFullConfirmedDeveloper", _bus.ExchangeDeclare(
                           "new_full_confirmed_developer_events", ExchangeType.Fanout));
            _exchanges.Add("NewProjectCreated", _bus.ExchangeDeclare(
                           "new_project_created_events", ExchangeType.Fanout));
            _exchanges.Add("AdminNotificationInfo", _bus.ExchangeDeclare(
                           "admin_notification_info_events", ExchangeType.Fanout));
            _exchanges.Add("PasswordChangeRequest", _bus.ExchangeDeclare(
                           "password_change_request_events", ExchangeType.Direct));
        }

        private void DeclareQueues()
        {
            _queues = new Dictionary<string, IQueue>();
            _queues.Add("ChangePassword", _bus.QueueDeclare(
                        "change_password_queue"));
            _queues.Add("NewContactMessageNotify", _bus.QueueDeclare(
                "new_contact_message_notify_queue"));
            _queues.Add("DeveloperHasLeftProjectNotify", _bus.QueueDeclare(
                        "developer_has_left_project_notify_queue"));
            _queues.Add("NewDeveloperOnProjectNotify", _bus.QueueDeclare(
                        "new_developer_on_project_notify_queue"));
            _queues.Add("NewEmailConfirmedDeveloperNotify", _bus.QueueDeclare(
                        "new_email_confirmed_developer_notify_queue"));
            _queues.Add("NewFullConfirmedDeveloperNotify", _bus.QueueDeclare(
                        "new_full_confirmed_developer_notify_queue"));
            _queues.Add("NewProjectCreatedNotify", _bus.QueueDeclare(
                        "new_project_created_notify_queue"));
            _queues.Add("AdminNotificationInfoNotify", _bus.QueueDeclare(
                        "admin_notification_info_notify_queue"));
            _queues.Add("ValidateMail", _bus.QueueDeclare(
                        "validate_mail_queue"));
        }

        private void DeclareBindings()
        {
            _bus.Bind(_exchanges["PasswordChangeRequest"], 
                      _queues["ChangePassword"], 
                                "change_password");
            _bus.Bind(_exchanges["Notification"],
                      _exchanges["NewContactMessage"],
                      "new_contact_message");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["DeveloperHasLeftProject"], 
                                "developer_has_left_project");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["NewDeveloperOnProject"], 
                                "new_developer_on_project");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["NewEmailConfirmedDeveloper"], 
                                "new_email_confirmed_developer");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["NewFullConfirmedDeveloper"], 
                                "new_full_confirmed_developer");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["NewProjectCreated"], 
                                "new_project_created");
            _bus.Bind(_exchanges["Notification"], 
                      _exchanges["AdminNotificationInfo"], 
                                "admin_notification_info");
            _bus.Bind(_exchanges["NewContactMessage"],
                      _queues["NewContactMessageNotify"],
                                "new_contact_message_notify");
            _bus.Bind(_exchanges["DeveloperHasLeftProject"], 
                      _queues["DeveloperHasLeftProjectNotify"], 
                                "developer_has_left_project_notify");
            _bus.Bind(_exchanges["NewDeveloperOnProject"], 
                      _queues["NewDeveloperOnProjectNotify"], 
                                "new_developer_on_project_notify");
            _bus.Bind(_exchanges["NewEmailConfirmedDeveloper"], 
                      _queues["NewEmailConfirmedDeveloperNotify"], 
                                "new_email_confirmed_developer_notify");
            _bus.Bind(_exchanges["NewFullConfirmedDeveloper"],
                      _queues["NewFullConfirmedDeveloperNotify"], 
                                "new_full_confirmed_developer_notify");
            _bus.Bind(_exchanges["NewProjectCreated"], 
                      _queues["NewProjectCreatedNotify"], 
                                "new_project_created_notify");
            _bus.Bind(_exchanges["AdminNotificationInfo"], 
                      _queues["AdminNotificationInfoNotify"], 
                                "admin_notification_info_notify");
            _bus.Bind(_exchanges["MailValidationRequest"], 
                      _queues["ValidateMail"], 
                                "validate_mail");
        }

        private readonly EventBusSettings _eventBusSettings;
        private IAdvancedBus _bus;
        private Dictionary<string, IExchange> _exchanges;
        private Dictionary<string, IQueue> _queues;
    }
}
