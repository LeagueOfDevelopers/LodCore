using UserManagement.Domain.Events;
using ProjectManagement.Domain.Events;
using NotificationService;
using ContactContext.Events;

namespace EventHandlers
{
    public interface INotificationsHandler
    {
        void NotifyAboutNewEmailConfirmedDeveloper(NewEmailConfirmedDeveloper @event);

        void NotifyAboutNewFullConfirmedDeveloper(NewFullConfirmedDeveloper @event);

        void NotifyAboutAdminNotificationInfo(AdminNotificationInfo @event);

        void NotifyAboutDeveloperHasLeftProject(DeveloperHasLeftProject @event);

        void NotifyAboutNewDeveloperOnProject(NewDeveloperOnProject @event);

        void NotifyAboutNewProjectCreated(NewProjectCreated @event);

        void NotifyAboutNewContactMessage(NewContactMessage @event);
    }
}
