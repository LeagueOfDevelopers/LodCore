using System.Web.Http.Filters;
using Serilog;

namespace LodCoreApi.App_Data
{
    public class ExceptionLogger : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var requestContent = actionExecutedContext.Request.Content.ReadAsStringAsync().Result;
            if (requestContent.Length > 200)
                requestContent = requestContent.Substring(0, 200);

            Log.Warning(actionExecutedContext.Exception,
                        "Exception {0} occured while processing request with content {1}",
                        actionExecutedContext.Exception.Message,
                        requestContent);
        }
    }
}
