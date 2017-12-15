using System.Web;
using System.Web.Http;
using DataAccess;
using SimpleInjector.Integration.WebApi;
using RabbitMQ.Client;

namespace FrontendServices
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new Bootstrapper().Configure();
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest()
        {
            var sessionProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(
                typeof (DatabaseSessionProvider)) as DatabaseSessionProvider;
            sessionProvider.OpenSession();
        }

        protected void Application_EndRequest()
        {
            var sessionProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(
                typeof (DatabaseSessionProvider)) as DatabaseSessionProvider;
            sessionProvider.CloseSession();
        }
    }
}