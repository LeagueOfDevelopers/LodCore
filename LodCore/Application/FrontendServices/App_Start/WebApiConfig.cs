using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;
using FrontendServices.Authorization;
using UserManagement.Application;

namespace FrontendServices
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

            var authorizer = config.DependencyResolver.GetService(typeof (IAuthorizer)) as IAuthorizer;
            config.Filters.Add(new AuthenticateAttribute(authorizer));

            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

        private static void ConfigureCrossDomainRequestsSupport(HttpConfiguration config)
        {
            var frontendDomain = ConfigurationManager.AppSettings["FrontendDomain"];
            var cors = new EnableCorsAttribute(frontendDomain, "*", "*")
            { SupportsCredentials = true };
            config.EnableCors(cors);
        }
    }
}