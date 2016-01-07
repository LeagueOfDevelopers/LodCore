using System.Web;
using System.Web.Http;
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
    }
}