using RabbitMQEventBus;
using UserManagement.Domain.Events;
using NotificationService;
using ProjectManagement.Domain.Events;
using ContactContext.Events;

namespace EventHandlers
{
    public class NotificationsHandler : INotificationsHandler
    {
        public NotificationsHandler(
            IEventSink userManagementEventSink,
            NotificationEventSink notificationEventSink,
            IEventBus eventBus)
        {
            _userManagementEventSink = userManagementEventSink;
            _notificationEventSink = notificationEventSink;
            _eventBus = eventBus;

            BindToConsumer();
        }

        public void NotifyAboutNewEmailConfirmedDeveloper(NewEmailConfirmedDeveloper @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutNewFullConfirmedDeveloper(NewFullConfirmedDeveloper @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutAdminNotificationInfo(AdminNotificationInfo @event)
        {
            _notificationEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutDeveloperHasLeftProject(DeveloperHasLeftProject @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutNewDeveloperOnProject(NewDeveloperOnProject @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutNewProjectCreated(NewProjectCreated @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
        }

        public void NotifyAboutNewContactMessage(NewContactMessage @event)
        {
            _userManagementEventSink.ConsumeEvent(@event);
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

        private readonly IEventSink _userManagementEventSink;
        private readonly NotificationEventSink _notificationEventSink;
        private readonly IEventBus _eventBus;
    }
}
