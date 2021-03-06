﻿using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using LodCoreApiOld.App_Data;
using LodCoreApiOld.Authorization;
using LodCoreLibraryOld.Domain.UserManagement;
using Newtonsoft.Json;

namespace LodCoreApiOld
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            ConfigureCrossDomainRequestsSupport(config);

            config.Routes.MapHttpRoute(
                "DefaultApi", "{controller}/{id}", new {id = RouteParameter.Optional}
            );

            var authorizer = config.DependencyResolver.GetService(typeof(IAuthorizer)) as IAuthorizer;
            config.Filters.Add(new AuthenticateAttribute(authorizer));
            config.Filters.Add(new ExceptionLogger());

            var isLocalLaunch = ConfigurationManager.AppSettings["LocalLaunch"];
            if (isLocalLaunch.Equals("false")) config.Filters.Add(new HttpsValidator());

            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
                PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

        private static void ConfigureCrossDomainRequestsSupport(HttpConfiguration config)
        {
            var frontendDomain = ConfigurationManager.AppSettings["FrontendDomain"];
            var cors = new EnableCorsAttribute(frontendDomain, "*", "*")
                {SupportsCredentials = true};
            config.EnableCors(cors);
        }
    }
}