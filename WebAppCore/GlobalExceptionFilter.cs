using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebAppCore
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        ILogger<GlobalExceptionFilter> logger = null;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> exceptionLogger)
        {
            logger = exceptionLogger;
        }

        public void OnException(ExceptionContext context)
        {
            // log the exception
            logger.LogError(0, context.Exception.GetBaseException(), "Exception occurred.");
        }
    }
}