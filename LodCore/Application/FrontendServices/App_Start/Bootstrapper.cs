using System;
using System.Configuration;
using System.Web.Http;
using Common;
using ContactContext;
using ContactContext.Events;
using DataAccess;
using DataAccess.Pagination;
using DataAccess.Repositories;
using FilesManagement;
using FrontendServices.App_Data;
using FrontendServices.App_Data.Mappers;
using Mailing;
using NotificationService;
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
using UserPresentaton;
using RabbitMQEventBus;
using Serilog;
using Gateway;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;
using System.Web;

namespace FrontendServices
{
    public class Bootstrapper
    {
        public Container Configure()
        {
            StartLogger();
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            RegisterSettings(container);
            container.Register<IDatabaseSessionProvider, DatabaseSessionProvider>(Lifestyle.Singleton);
            container.Register<IUserManager, UserManager>(Lifestyle.Singleton);
            container.Register<ProjectRepository>(Lifestyle.Singleton);
            container.Register<IPasswordManager, PasswordManager>(Lifestyle.Singleton);
            container.Register<IUserRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);

            //todo: replace to open-generic registration
            container.Register<IPaginableRepository<Account>>(
                () => new PaginableRepository<Account>(container.GetInstance<IDatabaseSessionProvider>()),
                Lifestyle.Singleton);
            container.Register<IPaginableRepository<Delivery>>(
               () => new PaginableRepository<Delivery>(container.GetInstance<IDatabaseSessionProvider>()),
               Lifestyle.Singleton);
            container.Register<IPaginableRepository<Project>>(
               () => new PaginableRepository<Project>(container.GetInstance<IDatabaseSessionProvider>()),
               Lifestyle.Singleton);

            container.Register<IPaginationWrapper<Account>>(
                () => new PaginationWrapper<Account>(container.GetInstance<IPaginableRepository<Account>>()),
                Lifestyle.Singleton);
            container.Register<IPaginationWrapper<Delivery>>(
               () => new PaginationWrapper<Delivery>(container.GetInstance<IPaginableRepository<Delivery>>()),
               Lifestyle.Singleton);
            container.Register<IPaginationWrapper<Project>>(
                () => new PaginationWrapper<Project>(container.GetInstance<IPaginableRepository<Project>>()),
                Lifestyle.Singleton);

            container.Register<IConfirmationService>(() => new ConfirmationService(
                container.GetInstance<IUserRepository>(),
                container.GetInstance<IValidationRequestsRepository>(),
                container.GetInstance<IEventPublisher>()), 
                Lifestyle.Singleton);
            container.Register<IEventRepository, EventRepository>(Lifestyle.Singleton);
            container.Register<IDistributionPolicyFactory, DistributionPolicyFactory>(Lifestyle.Singleton);
            container.Register<IPasswordChangeRequestRepository, PasswordChangeRequestRepository>(Lifestyle.Singleton);
            container.Register<IUsersRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<IProjectRelativesRepository>(() => container.GetInstance<ProjectRepository>(),
                Lifestyle.Singleton);
            container.Register<IUserRoleAnalyzer, UserRoleAnalyzer>(Lifestyle.Singleton);
            container.Register<INotificationEmailDescriber, NotificationEmailDescriber>(Lifestyle.Singleton);
            container.Register<INotificationService>(() =>
                new NotificationService.NotificationService(
                    container.GetInstance<IEventRepository>(),
                    container.GetInstance<PaginationSettings>()), Lifestyle.Singleton);
            container.Register<IImageResizer>(
                () => new ImageResizer(500, container.GetInstance<FileStorageSettings>(), 
                container.GetInstance<ApplicationLocationSettings>()), Lifestyle.Singleton);
            container.Register<IProjectProvider>(() =>
                new ProjectProvider(
                    container.GetInstance<IProjectRepository>(),
                    container.GetInstance<PaginationSettings>(),
                    container.GetInstance<IEventPublisher>()),
                Lifestyle.Singleton);
            container.Register<IProjectRepository>(
                () => container.GetInstance<ProjectRepository>(),
                Lifestyle.Singleton);
            container.Register<IContactsService>(() => new ContactsService(container.GetInstance<IEventPublisher>()), 
                Lifestyle.Singleton);
            container.Register<EventMapper>(Lifestyle.Singleton);
            container.Register<IValidationRequestsRepository, ValidationRequestsRepository>(Lifestyle.Singleton);
            container.Register<IFileManager, FileManager>(Lifestyle.Singleton);
            container.Register<IUserPresentationProvider, UserPresentationProvider>(Lifestyle.Singleton);
            container.Register<INotificationSettingsRepository, NotificationSettingsRepository>(Lifestyle.Singleton);
            container.Register<IAuthorizer>(() => new Authorizer(
                TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["Authorizer.TokenLifeTimeInSeconds"])),
                container.GetInstance<IUserRepository>()),
                Lifestyle.Singleton);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Register<IProjectMembershipRepostiory, ProjectMembershipRepository>(Lifestyle.Singleton);
            container.Register<IMailer, Mailer>(Lifestyle.Singleton);
            container.RegisterCollection<IEventSink>(new[] { typeof(UserManagementEventSink<>).Assembly,
                                                             typeof(NotificationEventSink<>).Assembly,
                                                             typeof(ProjectsEventSink<>).Assembly,
                                                             typeof(ContactsEventSink<>).Assembly });
            container.Register<EventConsumersContainer>(Lifestyle.Singleton);
            container.Register<IEventConsumersContainer>(() => container.GetInstance<EventConsumersContainer>(),
                                                                                    Lifestyle.Singleton);
            container.Register<IEventPublisherProvider>(() => container.GetInstance<EventConsumersContainer>(),
                                                                                    Lifestyle.Singleton);
            container.Register<IEventPublisher>(() => container.GetInstance<IEventPublisherProvider>()
                                                      .GetEventPublisher(), Lifestyle.Singleton);

            container.Register<IGithubGateway, GithubGateway>(Lifestyle.Singleton);

            var consumersContainer = container.GetInstance<EventConsumersContainer>();
            consumersContainer.StartListening();
            RegisterEventConsumers(container);

            container.Verify();
            return container;
        }

        private static void RegisterSettings(Container container)
        {
            var settings = ConfigurationManager.AppSettings;
            container.Register(() => SettingsReader.ReadGithubGatewaySettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadProfileSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadEventBusSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadMailerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadUserRoleAnalyzerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadFileStorageSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadConfirmationSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadNotificationsPaginationSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadRelativeEqualityComparerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadIssuePaginationSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadApplicationLocationSettings(settings), Lifestyle.Singleton);
        }

	    private static void RegisterEventConsumers(Container container)
	    {
            var consumersContainer = container.GetInstance<EventConsumersContainer>();

			consumersContainer.RegisterConsumer(container.GetInstance<ProjectsEventSink<NewDeveloperOnProject>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<UserManagementEventSink<NewEmailConfirmedDeveloper>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<UserManagementEventSink<NewFullConfirmedDeveloper>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<NotificationEventSink<AdminNotificationInfo>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<ProjectsEventSink<DeveloperHasLeftProject>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<ProjectsEventSink<NewProjectCreated>>());
		    consumersContainer.RegisterConsumer(container.GetInstance<ContactsEventSink<NewContactMessage>>());
			consumersContainer.RegisterConsumer(container.GetInstance<PasswordChangeHandler>());
			consumersContainer.RegisterConsumer(container.GetInstance<MailValidationHandler>());
	    }

        private static void StartLogger()
        {
            var rootPath = HttpContext.Current.Server.MapPath("logs");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(rootPath + @"\log-{Date}.log", shared: true)
                .CreateLogger(); 
            Log.Information("Logger started");
        }
    }
}