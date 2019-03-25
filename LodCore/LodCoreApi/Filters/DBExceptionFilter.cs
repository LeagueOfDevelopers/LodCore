using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace LodCoreApi.Filters
{
    public class DBExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception, "DBExcetionFilter");
        }
    }
}