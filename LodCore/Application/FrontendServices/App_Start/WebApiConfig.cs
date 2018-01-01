using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using FrontendServices.App_Data;
using FrontendServices.Authorization;
using UserManagement.Application;

namespace FrontendServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //ConfigureCrossDomainRequestsSupport(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi", "{controller}/{id}", new {id = RouteParameter.Optional}
                );

            var authorizer = config.DependencyResolver.GetService(typeof (IAuthorizer)) as IAuthorizer;
            config.Filters.Add(new AuthenticateAttribute(authorizer));
            config.Filters.Add(new ExceptionLogger());

            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

       /* private static void ConfigureCrossDomainRequestsSupport(HttpConfiguration config)
        {
            var frontendDomain = ConfigurationManager.AppSettings["FrontendDomain"];
            //todo: убрать звёздочку
            var cors = new EnableCorsAttribute($"{frontendDomain}", "*", "*");
            config.EnableCors(cors);
        }*/
    }
}