using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LodCore.Mappers;
using LodCore.Pagination;
using LodCore.Security;
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
using LodCoreLibrary.QueryService;
using LodCoreLibrary.QueryService.Handlers;
using LodCoreLibrary.QueryService.Queries;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace LodCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            StartLogger();
            ConfigureSecurity(services);

            DatabaseSessionProvider databaseSessionProvider = new DatabaseSessionProvider();
            IEventPublisherProvider eventPublisherProvider = new EventConsumersContainer(
                new EventBusSettings(Configuration.GetSection("EventBusSettings").GetValue<string>("HostName"), 
                Configuration.GetSection("EventBusSettings").GetValue<string>("VirtualHost"),
                Configuration.GetSection("EventBusSettings").GetValue<string>("UserName"),
                Configuration.GetSection("EventBusSettings").GetValue<string>("Password")), 
                databaseSessionProvider);
            
            IEventPublisher eventPublisher = eventPublisherProvider.GetEventPublisher();
            IWebSocketStreamProvider webSocketStreamProvider = new WebSocketStreamProvider();

            ProjectQueryHandler projectQueryHandler = new ProjectQueryHandler(Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));
            DeveloperQueryHandler developerQueryHandler = new DeveloperQueryHandler(Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));

            IValidationRequestsRepository validationRequestsRepository = new ValidationRequestsRepository(databaseSessionProvider);
            IUserRepository userRepository = new UserRepository(databaseSessionProvider);
            IProjectRepository projectRepository = new ProjectRepository(Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"),
                projectQueryHandler);
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

            services.AddSingleton(projectProvider);
            services.AddSingleton(projectsMapper);
            services.AddSingleton(userManager);
            services.AddSingleton(paginationProjectWrapper);
            services.AddSingleton(contactsService);
            services.AddSingleton(eventMapper);
            services.AddSingleton(notificationService);
            services.AddSingleton(paginationDeliveryWrapper);
            services.AddSingleton(projectQueryHandler);
            services.AddSingleton(developerQueryHandler);

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "LodCore API"
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.OperationFilter<ExamplesOperationFilter>();
                c.DescribeAllEnumsAsStrings();
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

        private void StartLogger()
        {
            var logglyConfig = LogglyConfig.Instance;
            logglyConfig.CustomerToken = Configuration.GetSection("Logging").GetValue<string>("CustomerToken");
            logglyConfig.ApplicationName = "LodBackend";

            logglyConfig.Transport.EndpointHostname = "logs-01.loggly.com";
            logglyConfig.Transport.EndpointPort = 6514;
            logglyConfig.Transport.LogTransport = LogTransport.SyslogSecure;

            var ct = new ApplicationNameTag { Formatter = "application-{0}" };
            logglyConfig.TagConfig.Tags.Add(ct);
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(Path.Combine(Env.ContentRootPath, @"logs/log-{Date}.log"), shared: true)
                .WriteTo.Loggly()
                .CreateLogger();
            
            Log.Information("Logger has started");
        }

        private void ConfigureSecurity(IServiceCollection services)
        {
            var securityConfiguration = Configuration.GetSection("Authorizer");
            var securitySettings = new SecuritySettings(
                securityConfiguration["EncryptionKey"], 
                securityConfiguration["Issue"],
                securityConfiguration.GetValue<TimeSpan>("ExpirationPeriod"));
            var jwtIssuer = new JwtIssuer(securitySettings);
            services.AddSingleton(securitySettings);
            services.AddSingleton<IJwtIssuer>(jwtIssuer);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(securitySettings.EncryptionKey))
                    };
                });

            services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy =
                        new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser().Build();

                    options.AddPolicy("AdminOnly",
                        new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                            .RequireClaim(Claims.Roles.RoleClaim, Claims.Roles.Admin).Build());
                });
        }
    }
}
