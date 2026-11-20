using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace E_commerce_System.filters
{
    public class ActionTimingFilter(ILogger<ActionTimingFilter>logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var stopwatch = Stopwatch.StartNew();

            try
            {
                await next();
            }
            catch (Exception ex)
            {
                logger.LogInformation(
                   "MVC action [ActionName] took [ElapsedMilleseconds] ms",
                   context.ActionDescriptor.DisplayName,
                   stopwatch.Elapsed.TotalMilliseconds);
            }
        }
    }
}
