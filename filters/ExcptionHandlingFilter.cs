using E_commerce_System.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_commerce_System.filters
{
    public class ExcptionHandlingFilter(ILogger<ExcptionHandlingFilter>logger,IHostEnvironment environment) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            logger.LogError(context.Exception,
                "unhandeled excption in MVC action {ActionName} for request {TraceId}",
                context.ActionDescriptor.DisplayName,
                context.HttpContext.TraceIdentifier);

            var problem = ExcptionProblemDetailsFactory.Create(context.Exception, context.HttpContext, environment);

            var result = new ObjectResult(problem)
            {
                StatusCode = StatusCodes.Status500InternalServerError,

            };

            result.ContentTypes.Add("appliction/problem+json");

            context.Result = result;
            context.ExceptionHandled = true;

        
        }
    }
}
