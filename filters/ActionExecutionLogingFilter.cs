using Microsoft.AspNetCore.Mvc.Filters;

namespace E_commerce_System.filters
{
    public class ActionExecutionLogingFilter(ILogger<ActionExecutionLogingFilter> logger) : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("starting MVC action {ActionName}",context.ActionDescriptor.DisplayName);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionname = context.ActionDescriptor.DisplayName;
            if(context.Exception is not null && !context.ExceptionHandled)
            {
                logger.LogError(context.Exception, "MVC action {ActionName} failed", actionname);
                return;
            }

            logger.LogInformation("finshing MVC action [ActionName]", actionname);
        }

       
    }
}
