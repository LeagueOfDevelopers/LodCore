using System.Diagnostics;
using System.Web;
using System.Web.Http;
using DataAccess;
using SimpleInjector.Integration.WebApi;

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
            Debug.WriteLine("Begin request");
            var sessionProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(
                typeof (DatabaseSessionProvider)) as DatabaseSessionProvider;
            sessionProvider.OpenSession();
        }

        protected void Application_EndRequest()
        {
            Debug.WriteLine("End request");
            var sessionProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(
                typeof (DatabaseSessionProvider)) as DatabaseSessionProvider;
            sessionProvider.CloseSession();
        }
    }
}