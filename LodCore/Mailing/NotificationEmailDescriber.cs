using Journalist;
using NotificationService;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using UserManagement.Domain.Events;

namespace Mailing
{
    public class NotificationEmailDescriber : INotificationEmailDescriber
    {
        public NotificationEmailDescriber(IOrderRepository orderRepository, IProjectRepository projectRepository, IUserManager userManager)
        {
            _orderRepository = orderRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        public string Describe(IEventInfo @eventInfo)
        {
            Require.NotNull(@eventInfo, nameof(@eventInfo));

            return Describe((dynamic) @eventInfo);
        }

        private string Describe(DeveloperHasLeftProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userManager.GetUser(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.DeveloperHasLeftProject, developerFullName, project.Name);
        }

        private string Describe(NewDeveloperOnProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userManager.GetUser(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.NewDeveloperOnProject, project.Name, developerFullName);
        }

        private string Describe(NewEmailConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userManager.GetUser(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            return string.Format(EventDescriptionResources.NewEmailConfirmedDeveloper, developerFullName, developer.Email.Address);
        }

        private string Describe(NewFullConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userManager.GetUser(@event.NewDeveloperId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            return string.Format(EventDescriptionResources.NewFullConfirmedDeveloper, developerFullName);
        }

        private string Describe(NewProjectCreated @event)
        {
            Require.NotNull(@event, nameof(@event));

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.NewProjectCreated, project.Name, project.Info);
        }

        private string Describe(OrderPlaced @event)
        {
            var order = _orderRepository.GetOrder(@event.OrderId);

            return string.Format(EventDescriptionResources.OrderPlaced, order.CreatedOnDateTime.Date, order.Header,
                order.Email, order.Description);
        }

        private readonly IProjectRepository _projectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserManager _userManager;
    }
}