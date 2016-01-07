using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FrontendServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            ConfigureCrossDomainRequestsSupport(config);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi", "{controller}/{id}", new {id = RouteParameter.Optional}
                );
        }

        private static void ConfigureCrossDomainRequestsSupport(HttpConfiguration config)
        {
            var fronendDomain = ConfigurationManager.AppSettings["FrontendDomain"];
            var cors = new EnableCorsAttribute(fronendDomain, "*", "*");
            config.EnableCors(cors);
        }
    }
}