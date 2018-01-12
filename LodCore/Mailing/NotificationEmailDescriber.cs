﻿using Common;
using ContactContext.Events;
using Journalist;
using NotificationService;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;
using UserManagement.Domain.Events;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;

namespace Mailing
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

        public string Describe(IEventInfo @eventInfo)
        {
            Require.NotNull(@eventInfo, nameof(@eventInfo));

            return Describe((dynamic) @eventInfo);
        }

        private string Describe(NewContactMessage @event)
        {
            Require.NotNull(@event, nameof(@event));

            return string.Format(EventDescriptionResources.NewContactMessage, @event.ClientName, @event.MessageTopic,
                @event.MessageBody, @event.ClientEmailAddress);
        }

        private string Describe(DeveloperHasLeftProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.DeveloperHasLeftProject, developerFullName, project.Name);
        }

        private string Describe(NewDeveloperOnProject @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.NewDeveloperOnProject, project.Name, developerFullName);
        }

        private string Describe(NewEmailConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.UserId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            return string.Format(EventDescriptionResources.NewEmailConfirmedDeveloper, developerFullName,
                developer.Email.Address);
        }

        private string Describe(NewFullConfirmedDeveloper @event)
        {
            Require.NotNull(@event, nameof(@event));

            var developer = _userRepository.GetAccount(@event.NewDeveloperId);
            var developerFullName = developer.Firstname + " " + developer.Lastname;

            return string.Format(EventDescriptionResources.NewFullConfirmedDeveloper, developerFullName);
        }

        private string Describe(NewProjectCreated @event)
        {
            Require.NotNull(@event, nameof(@event));

            var project = _projectRepository.GetProject(@event.ProjectId);

            return string.Format(EventDescriptionResources.NewProjectCreated, project.Name, project.Info);
        }

        private string Describe(AdminNotificationInfo @event)
        {
            return string.Format(@event.InfoText);
        }
    }
}