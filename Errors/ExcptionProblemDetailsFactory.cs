using Microsoft.AspNetCore.Mvc;

namespace E_commerce_System.Errors
{
    /// <summary>
    /// Provides a factory for creating ProblemDetails instances representing exceptions in API responses.
    /// </summary>
    /// <remarks>Populates ProblemDetails with status, title, detail, instance, and trace identifier based on
    /// the exception, HTTP context, and hosting environment.</remarks>
    public static class ExcptionProblemDetailsFactory
    {
        public static ProblemDetails Create(Exception exception,HttpContext context,IHostEnvironment environment)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "un expcted error",
                Detail = environment.IsDevelopment() ? exception.Message : "please try agin later",
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;
            return problem;

        }
    }
}
