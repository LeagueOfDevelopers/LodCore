using System.Diagnostics;
using System.Web.Http.Filters;

namespace FrontendServices.App_Data
{
    public class ExceptionLogger : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var requestContent = actionExecutedContext.Request.Content.ReadAsStringAsync().Result;
            if (requestContent.Length > 200)
            {
                requestContent = requestContent.Substring(0, 200);
            }

            Trace.WriteLine(
                string.Format("Exception {0} occured while processing request with content \n{1}\nStackTrace:\n{2}",
                    actionExecutedContext.Exception.Message,
                    requestContent,
                    actionExecutedContext.Exception.StackTrace));
        }
    }
}