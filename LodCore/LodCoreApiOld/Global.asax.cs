using System.Web;
using System.Web.Http;
using SimpleInjector.Integration.WebApi;
using LodCoreLibraryOld.Common;

namespace LodCoreApiOld
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
                typeof(IDatabaseSessionProvider)) as IDatabaseSessionProvider;
            sessionProvider.OpenSession();
        }

        protected void Application_EndRequest()
        {
            var sessionProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(
                typeof(IDatabaseSessionProvider)) as IDatabaseSessionProvider;
            sessionProvider.CloseSession();
        }
    }
}