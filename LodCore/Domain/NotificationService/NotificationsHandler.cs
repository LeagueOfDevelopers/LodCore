using Common;
using ContactContext.Events;
using ProjectManagement.Domain.Events;
using RabbitMQEventBus;
using UserManagement.Domain.Events;

namespace NotificationService
{
    public class NotificationsHandler : 
		IEventConsumer<NewEmailConfirmedDeveloper>,
	    IEventConsumer<NewFullConfirmedDeveloper>,
	    IEventConsumer<AdminNotificationInfo>,
	    IEventConsumer<DeveloperHasLeftProject>,
	    IEventConsumer<NewDeveloperOnProject>,
	    IEventConsumer<NewProjectCreated>,
	    IEventConsumer<NewContactMessage>
	{
        public NotificationsHandler(EventSinkBase eventSink)
        {
            _eventSink = eventSink;
        }

        public void NotifyAboutNewEvent<T>(T @event)
            where T : class
        {
            _eventSink.ConsumeEvent((IEventInfo)@event);
        }

		public void Consume(NewEmailConfirmedDeveloper @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(NewFullConfirmedDeveloper @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(AdminNotificationInfo @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(DeveloperHasLeftProject @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(NewDeveloperOnProject @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(NewProjectCreated @event)
		{
			NotifyAboutNewEvent(@event);
		}

		public void Consume(NewContactMessage @event)
		{
			NotifyAboutNewEvent(@event);
		}

		private readonly EventSinkBase _eventSink;
	}
}
