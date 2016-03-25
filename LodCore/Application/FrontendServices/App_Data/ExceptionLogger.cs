using System;
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
                string.Format("\n{0}: Exception {1} occured while processing request with content \n{2}\nStackTrace:\n{3}",
                    DateTime.Now,
                    actionExecutedContext.Exception.Message,
                    requestContent,
                    actionExecutedContext.Exception.StackTrace));
        }
    }
}