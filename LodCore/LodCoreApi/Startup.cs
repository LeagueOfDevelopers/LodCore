﻿using System;
using System.IO;
using System.Text;
using LodCore.Common;
using LodCore.Domain.NotificationService;
using LodCore.Domain.ProjectManagment;
using LodCore.Domain.UserManagement;
using LodCore.Facades;
using LodCore.Infrastructure.ContactContext;
using LodCore.Infrastructure.DataAccess.Pagination;
using LodCore.Infrastructure.DataAccess.Repositories;
using LodCore.Infrastructure.EventBus;
using LodCore.Infrastructure.WebSocketConnection;
using LodCore.QueryService.Handlers;
using LodCoreApi.Filters;
using LodCoreApi.Mappers;
using LodCoreApi.Pagination;
using LodCoreApi.Security;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace LodCoreApi
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
            var origins = Configuration.GetValue<string>("Origins").Split(',');
            services.AddCors(options =>
            {
                options.AddPolicy("CommonPolicy", builder =>
                    builder.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            var eventConsumersContainer = new EventConsumersContainer(
                new EventBusSettings(Configuration.GetSection("EventBusSettings").GetValue<string>("HostName"),
                    Configuration.GetSection("EventBusSettings").GetValue<string>("VirtualHost"),
                    Configuration.GetSection("EventBusSettings").GetValue<string>("UserName"),
                    Configuration.GetSection("EventBusSettings").GetValue<string>("Password")));
            eventConsumersContainer.StartListening();

            var eventPublisher = eventConsumersContainer.GetEventPublisher();
            IWebSocketStreamProvider webSocketStreamProvider = new WebSocketStreamProvider();

            var projectQueryHandler =
                new ProjectQueryHandler(Configuration.GetSection("DatabaseSettings")
                    .GetValue<string>("ConnectionString"));
            var developerQueryHandler =
                new DeveloperQueryHandler(Configuration.GetSection("DatabaseSettings")
                    .GetValue<string>("ConnectionString"));

            IValidationRequestsRepository validationRequestsRepository = new ValidationRequestsRepository();
            IUserRepository userRepository =
                new UserRepository(Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));
            IProjectRepository projectRepository = new ProjectRepository(
                Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"),
                projectQueryHandler);
            IProjectMembershipRepostiory projectMembershipRepostiory = new ProjectMembershipRepository();
            IPasswordChangeRequestRepository passwordChangeRequestRepository = new PasswordChangeRequestRepository();
            IEventRepository eventRepository = new EventRepository(webSocketStreamProvider);

            IConfirmationService confirmationService =
                new ConfirmationService(userRepository, validationRequestsRepository, eventPublisher);
            IPasswordManager passwordManager = new PasswordManager(passwordChangeRequestRepository, userRepository);
            IProjectProvider projectProvider = new ProjectProvider(projectRepository, eventPublisher);
            IUserManager userManager = new UserManager(userRepository,
                confirmationService,
                new ProjectPaginationSettings(10),
                projectMembershipRepostiory,
                new ApplicationLocationSettings("backend", "frontend"),
                passwordManager,
                eventPublisher);

            var notificationHandler = new NotificationHandler(
                Configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"),
                Configuration.GetSection("NotificationPaginationSettings").GetValue<int>("PageSize"));
            INotificationService notificationService =
                new LodCore.Domain.NotificationService.NotificationService(eventRepository, new PaginationSettings(10));
            var projectsMapper = new ProjectsMapper(userManager);
            var eventMapper = new EventMapper(notificationService);
            IContactsService contactsService = new ContactsService(eventPublisher);

            IPaginableRepository<Delivery> paginableDeliveryRepository = new PaginableRepository<Delivery>();
            IPaginationWrapper<Delivery> paginationDeliveryWrapper =
                new PaginationWrapper<Delivery>(paginableDeliveryRepository);
            IPaginableRepository<Project> paginableProjectRepository = new PaginableRepository<Project>();
            IPaginationWrapper<Project> paginationProjectWrapper =
                new PaginationWrapper<Project>(paginableProjectRepository);


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
            services.AddSingleton(notificationHandler);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options => { options.Filters.Add(typeof(DBExceptionFilter)); });

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

            app.UseCors("CommonPolicy");

            app.UseStaticFiles();
            app.UseMvc();


            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LodCore API"); });
        }

        private void StartLogger()
        {
            var logglyConfig = LogglyConfig.Instance;
            logglyConfig.CustomerToken = Configuration.GetSection("Logging").GetValue<string>("CustomerToken");
            logglyConfig.ApplicationName = "LodBackend";

            logglyConfig.Transport.EndpointHostname = "logs-01.loggly.com";
            logglyConfig.Transport.EndpointPort = 6514;
            logglyConfig.Transport.LogTransport = LogTransport.SyslogSecure;

            var ct = new ApplicationNameTag {Formatter = "application-{0}"};
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