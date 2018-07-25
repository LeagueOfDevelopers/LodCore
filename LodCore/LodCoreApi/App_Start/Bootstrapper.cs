using System;
using System.Configuration;
using System.Web.Http;
using LodCoreApi.App_Data;
using LodCoreApi.App_Data.Mappers;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Serilog;
using System.Web;
using Loggly;
using Loggly.Config;
using LodCoreLibrary.Common;
using LodCoreLibrary.Infrastructure.DataAccess;
using LodCoreLibrary.Facades;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;
using LodCoreLibrary.Domain.NotificationService;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.Infrastructure.EventBus;
using LodCoreLibrary.Infrastructure.Mailing;
using LodCoreLibrary.Infrastructure.FilesManagement;
using LodCoreLibrary.Infrastructure.ContactContext;
using LodCoreLibrary.Infrastructure.WebSocketConnection;
using LodCoreLibrary.Infrastructure.Gateway;
using NotificationService;

namespace LodCoreApi
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
            container.Register<EventRepository>(Lifestyle.Singleton);
            container.Register<IEventRepository>(() => container.GetInstance<EventRepository>(), Lifestyle.Singleton);
            container.Register<INumberOfNotificationsProvider>(() => container.GetInstance<EventRepository>(), Lifestyle.Singleton);
            container.Register<IDistributionPolicyFactory, DistributionPolicyFactory>(Lifestyle.Singleton);
            container.Register<IPasswordChangeRequestRepository, PasswordChangeRequestRepository>(Lifestyle.Singleton);
            container.Register<IUsersRepository>(() => container.GetInstance<UserRepository>(), Lifestyle.Singleton);
            container.Register<IProjectRelativesRepository>(() => container.GetInstance<ProjectRepository>(),
                Lifestyle.Singleton);
            container.Register<IUserRoleAnalyzer, UserRoleAnalyzer>(Lifestyle.Singleton);
            container.Register<INotificationEmailDescriber, NotificationEmailDescriber>(Lifestyle.Singleton);
            container.Register<INotificationService>(() =>
                new LodCoreLibrary.Domain.NotificationService.NotificationService(
                    container.GetInstance<IEventRepository>(),
                    container.GetInstance<PaginationSettings>()), Lifestyle.Singleton);
            container.Register<IImageResizer>(
                () => new ImageResizer(500, container.GetInstance<FileStorageSettings>(), 
                container.GetInstance<ApplicationLocationSettings>()), Lifestyle.Singleton);
            container.Register<IProjectProvider>(() =>
                new ProjectProvider(
                    container.GetInstance<IProjectRepository>(),
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
            container.Register<IWebSocketStreamProvider, WebSocketStreamProvider>(Lifestyle.Singleton);
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
            //consumersContainer.StartListening();
            //RegisterEventConsumers(container);

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
            container.Register(() => SettingsReader.ReadProjectPaginationSettings(settings), Lifestyle.Singleton);
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
	        var logglyConfig = LogglyConfig.Instance;
	        logglyConfig.CustomerToken = ConfigurationManager.AppSettings["Loggly.CustomerToken"];
	        logglyConfig.ApplicationName = "LodBackend";
	        logglyConfig.Transport.EndpointHostname = "logs-01.loggly.com";
	        logglyConfig.Transport.EndpointPort = 6514;
	        logglyConfig.Transport.LogTransport = LogTransport.SyslogSecure;
	        var ct = new ApplicationNameTag {Formatter = "application-{0}"};
			logglyConfig.TagConfig.Tags.Add(ct);

			var rootPath = HttpContext.Current.Server.MapPath("logs");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(rootPath + @"\log-{Date}.log", shared: true)
				.WriteTo.Loggly()
                .CreateLogger(); 
            Log.Information("Logger has started");
        }
    }
}