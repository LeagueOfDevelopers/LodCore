using Journalist;
using LodCoreLibraryOld.Domain.NotificationService;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;

namespace LodCoreLibraryOld.Infrastructure.Mailing
{
    public class NotificationEmailDescriber : INotificationEmailDescriber
    {
        private readonly IProjectRepository _projectRepository;

        private readonly IUserRepository _userRepository;

        public NotificationEmailDescriber(
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public string Describe(string userName, IEventInfo @eventInfo)
        {
            Require.NotNull(@eventInfo, nameof(@eventInfo));
            Require.NotEmpty(userName, nameof(userName));

            return Describe(userName, (dynamic) @eventInfo);
        }

        private string Describe(string userName, NewContactMessage @event)
        {
            Require.NotNull(@event, nameof(@event));
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.NewContactMessage(
                userName, @event.ClientName, @event.MessageTopic, @event.MessageBody, @event.ClientEmailAddress));
            return template;
        }

        private string Describe(string userName, DeveloperHasLeftProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;
            var project = _projectRepository.GetProject(@event.ProjectId);
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.DeveloperHasLeftProject(
                userName, developerFullName, project.Name));
            return template;
        }

        private string Describe(string userName, NewDeveloperOnProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;
            var project = _projectRepository.GetProject(@event.ProjectId);
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.NewDeveloperOnProject(
                userName, developerFullName, project.Name));
            return template;
        }

        private string Describe(string userName, NewEmailConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.NewEmailConfirmedDeveloper(
                userName, developerFullName, developer.Email.Address));
            return template;
        }

        private string Describe(string userName, NewFullConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.NewDeveloperId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.NewFullConfirmedDeveloper(
                userName, developerFullName));
            return template;
        }

        private string Describe(string userName, NewProjectCreated @event)
        {
            Require.NotNull(@event, nameof(@event));
            var project = _projectRepository.GetProject(@event.ProjectId);
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.NewProject(
                userName, project.Name, project.Info));
            return template;
        }

        private string Describe(string userName, AdminNotificationInfo @event)
        {
            var template = RenderEmailTemplateHelper.RenderPartialToString(new EmailModels.AdminNotification(
                userName, @event.InfoText));
            return template;
        }
    }
}