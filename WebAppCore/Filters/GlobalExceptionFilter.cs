using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApp.DAL;
using WebApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WebAppCore
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute//IExceptionFilter
    {
     
        public GlobalExceptionFilter()
        {
            
        }
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();

            // log the exception
            // logger.LogError(0, context.Exception.GetBaseException(), "Exception occurred.");
            try
            {
                dbContext.ErrorLogs.Add(
                    new ErrorLog
                    {
                        Username = context.HttpContext.User?.Identity?.Name,
                        Path = context.HttpContext.Request?.Path,
                        Exception = context.Exception.ToString()

                    }
                    );
             await   dbContext.SaveChangesAsync();
            }
            catch { }
        }

        
    }
}