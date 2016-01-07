using System.Collections.Specialized;
using System.Configuration;
using System.Web.Http;
using DataAccess;
using DataAccess.Repositories;
using FrontendServices.App_Data;
using Gateways;
using Gateways.Redmine;
using Mailing;
using NotificationService;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using UserManagement.Application;
using UserManagement.Domain;
using UserManagement.Domain.Events;
using UserManagement.Infrastructure;

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
            container.Register<IRedmineUserRegistrar, ProjectManagerGateway>(Lifestyle.Singleton);
            container.Register<IGitlabUserRegistrar, VersionControlSystemGateway>(Lifestyle.Singleton);
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
            container.Register<IMailer, Mailer>(Lifestyle.Singleton);
            container.Register<IValidationRequestsRepository, ValidationRequestsRepository>(Lifestyle.Singleton);
            container.Register<DatabaseSessionProvider>(Lifestyle.Singleton);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();
            return container;
        }

        private static void RegisterSettings(Container container)
        {
            var settings = ConfigurationManager.AppSettings;
            container.Register(() => SettingsReader.ReadMailerSettings(settings), Lifestyle.Singleton);
            container.Register(() => SettingsReader.ReadRedmineSettings(settings), Lifestyle.Singleton);
        }
    }
}