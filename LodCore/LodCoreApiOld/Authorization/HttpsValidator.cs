using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LodCoreApiOld.Authorization
{
    public class HttpsValidator : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent
                    ("<p>Use https instead of http</p>", Encoding.UTF8, "text/html");

                var uriBuilder = new UriBuilder(actionContext.Request.RequestUri);
                uriBuilder.Scheme = Uri.UriSchemeHttps;
                uriBuilder.Port = 44379;

                actionContext.Response.Headers.Location = uriBuilder.Uri;
            }

            base.OnAuthorization(actionContext);
        }
    }
}