using System;
using System.Configuration;
using System.Web.Http;
using ContactContext;
using ContactContext.Events;
using DataAccess;
using DataAccess.Repositories;
using FilesManagement;
using FrontendServices.App_Data;
using Gateways;
using Gateways.Redmine;
using Mailing;
using NotificationService;
using OrderManagement.Infrastructure;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Domain.Events;
using UserManagement.Infrastructure;
using IMailer = UserManagement.Application.IMailer;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;

namespace FrontendServices
{
    public class Bootstrapper
    {
        public Container Configure()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            RegisterSettings(container);
            container.Register<IUserManager, UserManager>(Lifestyle.Singleton);
            container.Register<UserRepository>(Lifestyle.Singleton);
            container.Register<ProjectRepository>(Lifestyle.Singleton);
            container.Register<IUserRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<ProjectManagerGateway>(Lifestyle.Singleton);
            container.Register<IRedmineUserRegistrar>(() => container.GetInstance<ProjectManagerGateway>(), Lifestyle.Singleton);
            container.Register<IProjectManagerGateway>(() => container.GetInstance<ProjectManagerGateway>(), Lifestyle.Singleton);
            container.Register<VersionControlSystemGateway>(Lifestyle.Singleton);
            container.Register<IGitlabUserRegistrar>(() => container.GetInstance<VersionControlSystemGateway>(), Lifestyle.Singleton);
            container.Register<IVersionControlSystemGateway>(() => container.GetInstance<VersionControlSystemGateway>(), Lifestyle.Singleton);
            container.Register<IConfirmationService>(
                ()=> new ConfirmationService(
                    container.GetInstance<IUserRepository>(), 
                    container.GetInstance<IMailer>(), 
                    container.GetInstance<IValidationRequestsRepository>(), 
                    container.GetInstance<UserManagementEventSink>()), 
                Lifestyle.Singleton);
            container.Register<UserManagementEventSink>(Lifestyle.Singleton);
            container.Register<IEventRepository, EventRepository>(Lifestyle.Singleton);
            container.Register<IDistributionPolicyFactory, DistributionPolicyFactory>(Lifestyle.Singleton);
            container.Register<IUsersRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<IProjectRelativesRepository>(() => container.GetInstance<ProjectRepository>(), Lifestyle.Singleton);
            container.Register<Mailer>(Lifestyle.Singleton);
            container.Register<NotificationService.IMailer>(() => container.GetInstance<Mailer>(), Lifestyle.Singleton);
            container.Register<IMailer>(() => container.GetInstance<Mailer>(), Lifestyle.Singleton);
            container.Register<IUserRoleAnalyzer, UserRoleAnalyzer>(Lifestyle.Singleton);
            container.Register<INotificationEmailDescriber, NotificationEmailDescriber>(Lifestyle.Singleton);
            container.Register<IOrderRepository, OrderRepository>(Lifestyle.Singleton);
            container.Register<IProjectProvider>(() =>
                new ProjectProvider(
                    container.GetInstance<IProjectManagerGateway>(),
                    container.GetInstance<IVersionControlSystemGateway>(),
                    container.GetInstance<IProjectRepository>(),
                    container.GetInstance<ProjectsEventSink>(),
                    container.GetInstance<ProjectManagement.Infrastructure.IUserRepository>(),
                    container.GetInstance<PaginationSettings>()),
                Lifestyle.Singleton);
            container.Register<ProjectManagement.Infrastructure.IUserRepository>(
                () => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<IProjectRepository>(
                () => container.GetInstance<ProjectRepository>(), 
                Lifestyle.Singleton);
            container.Register<ContactsEventSink>(Lifestyle.Singleton);
            container.Register<IContactsService>(
                () => new ContactsService(
                    container.GetInstance<ContactsEventSink>()), 
                Lifestyle.Singleton);
            container.Register<IValidationRequestsRepository, ValidationRequestsRepository>(Lifestyle.Singleton);
            container.Register<DatabaseSessionProvider>(Lifestyle.Singleton);
            container.Register<IFileManager, FileManager>(Lifestyle.Singleton);
            container.Register<IAuthorizer>(() => new Authorizer(
                TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["Authorizer.TokenLifeTimeInSeconds"])),
                container.GetInstance<IUserRepository>()));
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();
            return container;
        }

        private static void RegisterSettings(Container container)
        {
            var settings = ConfigurationManager.AppSettings;
            container.Register(() => SettingsReader.ReadMailerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadRedmineSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadUserRoleAnalyzerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadGitlabSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadFileStorageSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadPaginationSettings(settings), Lifestyle.Singleton);
        }
    }
}