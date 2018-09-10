using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LodCore.Mappers;
using LodCore.Pagination;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.NotificationService;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Facades;
using LodCoreLibrary.Infrastructure.ContactContext;
using LodCoreLibrary.Infrastructure.DataAccess;
using LodCoreLibrary.Infrastructure.DataAccess.Pagination;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Infrastructure.EventBus;
using LodCoreLibrary.Infrastructure.WebSocketConnection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace LodCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            DatabaseSessionProvider databaseSessionProvider = new DatabaseSessionProvider();
            IEventPublisherProvider eventPublisherProvider = new EventConsumersContainer(
                new EventBusSettings("localhost", "/", "guest", "guest"), databaseSessionProvider);
            IEventPublisher eventPublisher = eventPublisherProvider.GetEventPublisher();
            IWebSocketStreamProvider webSocketStreamProvider = new WebSocketStreamProvider();

            IValidationRequestsRepository validationRequestsRepository = new ValidationRequestsRepository(databaseSessionProvider);
            IUserRepository userRepository = new UserRepository(databaseSessionProvider);
            IProjectRepository projectRepository = new ProjectRepository(databaseSessionProvider);
            IProjectMembershipRepostiory projectMembershipRepostiory = new ProjectMembershipRepository(databaseSessionProvider);
            IPasswordChangeRequestRepository passwordChangeRequestRepository = new PasswordChangeRequestRepository(databaseSessionProvider);
            IEventRepository eventRepository = new EventRepository(databaseSessionProvider, webSocketStreamProvider);

            IConfirmationService confirmationService = new ConfirmationService(userRepository, validationRequestsRepository, eventPublisher);
            IPasswordManager passwordManager = new PasswordManager(passwordChangeRequestRepository, userRepository);
            IProjectProvider projectProvider = new ProjectProvider(projectRepository, eventPublisher);
            IUserManager userManager = new UserManager(userRepository, 
                confirmationService,
                new ProjectPaginationSettings(10), 
                projectMembershipRepostiory, 
                new ApplicationLocationSettings("backend", "frontend"),
                passwordManager,
                eventPublisher);
            INotificationService notificationService = new LodCoreLibrary.Domain.NotificationService.NotificationService(eventRepository, new PaginationSettings(10));
            ProjectsMapper projectsMapper = new ProjectsMapper(userManager);
            EventMapper eventMapper = new EventMapper(notificationService);
            IContactsService contactsService = new ContactsService(eventPublisher);

            IPaginableRepository<Delivery> paginableDeliveryRepository = new PaginableRepository<Delivery>(databaseSessionProvider);
            IPaginationWrapper<Delivery> paginationDeliveryWrapper = new PaginationWrapper<Delivery>(paginableDeliveryRepository);
            IPaginableRepository<Project> paginableProjectRepository = new PaginableRepository<Project>(databaseSessionProvider);
            IPaginationWrapper<Project> paginationProjectWrapper = new PaginationWrapper<Project>(paginableProjectRepository);

            services.AddSingleton<IProjectProvider>(projectProvider);
            services.AddSingleton<ProjectsMapper>(projectsMapper);
            services.AddSingleton<IUserManager>(userManager);
            services.AddSingleton<IPaginationWrapper<Project>>(paginationProjectWrapper);
            services.AddSingleton<IContactsService>(contactsService);
            services.AddSingleton<EventMapper>(eventMapper);
            services.AddSingleton<INotificationService>(notificationService);
            services.AddSingleton<IPaginationWrapper<Delivery>>(paginationDeliveryWrapper);

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "LodCore API"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LodCore API");
            });
        }
    }
}
