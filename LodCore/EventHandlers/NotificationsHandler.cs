using RabbitMQEventBus;
using UserManagement.Domain.Events;
using NotificationService;
using ProjectManagement.Domain.Events;
using ContactContext.Events;
using DataAccess;

namespace EventHandlers
{
    public class NotificationsHandler : INotificationsHandler
    {
        public NotificationsHandler(
            EventSinkBase eventSink,
            DatabaseSessionProvider databaseSessionProvider,
            IEventBus eventBus)
        {
            _eventSink = eventSink;
            _databaseSessionProvider = databaseSessionProvider;
            _eventBus = eventBus;

            BindToConsumer();
        }

        public void NotifyAboutNewEmailConfirmedDeveloper(NewEmailConfirmedDeveloper @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutNewFullConfirmedDeveloper(NewFullConfirmedDeveloper @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutAdminNotificationInfo(AdminNotificationInfo @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutDeveloperHasLeftProject(DeveloperHasLeftProject @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutNewDeveloperOnProject(NewDeveloperOnProject @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutNewProjectCreated(NewProjectCreated @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutNewContactMessage(NewContactMessage @event)
        {
            NotifyAboutNewEvent(@event);
        }

        public void NotifyAboutNewEvent<T>(T @event)
            where T : class
        {
            _databaseSessionProvider.OpenSession();
            _eventSink.ConsumeEvent((IEventInfo)@event);
            _databaseSessionProvider.CloseSession();
        }

        private void BindToConsumer()
        {
            _notifyAboutNewEmailConfirmedDeveloper notifyAboutNewEmailConfirmedDeveloper =
                NotifyAboutNewEmailConfirmedDeveloper;
            _eventBus.SetConsumer("NewEmailConfirmedDeveloperNotify", notifyAboutNewEmailConfirmedDeveloper);

            _notifyAboutNewFullConfirmedDeveloper notifyAboutNewFullConfirmedDeveloper =
                NotifyAboutNewFullConfirmedDeveloper;
            _eventBus.SetConsumer("NewFullConfirmedDeveloperNotify", notifyAboutNewFullConfirmedDeveloper);

            _notifyAboutAdminNotificationInfo notifyAboutAdminNotificationInfo =
                NotifyAboutAdminNotificationInfo;
            _eventBus.SetConsumer("AdminNotificationInfoNotify", notifyAboutAdminNotificationInfo);

            _notifyAboutDeveloperHasLeftProject notifyAboutDeveloperHasLeftProject =
                NotifyAboutDeveloperHasLeftProject;
            _eventBus.SetConsumer("DeveloperHasLeftProjectNotify", notifyAboutDeveloperHasLeftProject);

            _notifyAboutNewDeveloperOnProject notifyAboutNewDeveloperOnProject =
                NotifyAboutNewDeveloperOnProject;
            _eventBus.SetConsumer("NewDeveloperOnProjectNotify", notifyAboutNewDeveloperOnProject);

            _notifyAboutNewProjectCreated notifyAboutNewProjectCreated =
                NotifyAboutNewProjectCreated;
            _eventBus.SetConsumer("NewProjectCreatedNotify", notifyAboutNewProjectCreated);

            _notifyAboutNewContactMessage notifyAboutNewContactMessage =
                NotifyAboutNewContactMessage;
            _eventBus.SetConsumer("NewContactMessageNotify", notifyAboutNewContactMessage);
        }

        private delegate void _notifyAboutNewEmailConfirmedDeveloper(NewEmailConfirmedDeveloper @event);
        private delegate void _notifyAboutNewFullConfirmedDeveloper(NewFullConfirmedDeveloper @event);
        private delegate void _notifyAboutAdminNotificationInfo(AdminNotificationInfo @event);
        private delegate void _notifyAboutDeveloperHasLeftProject(DeveloperHasLeftProject @event);
        private delegate void _notifyAboutNewDeveloperOnProject(NewDeveloperOnProject @event);
        private delegate void _notifyAboutNewProjectCreated(NewProjectCreated @event);
        private delegate void _notifyAboutNewContactMessage(NewContactMessage @event);

        private readonly EventSinkBase _eventSink;
        private readonly DatabaseSessionProvider _databaseSessionProvider;
        private readonly IEventBus _eventBus;
    }
}
