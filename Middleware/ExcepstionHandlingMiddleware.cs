using E_commerce_System.Errors;

namespace E_commerce_System.Middleware
{
    public class ExcepstionHandlingMiddleware(RequestDelegate next,ILogger<ExcepstionHandlingMiddleware>logger,IHostEnvironment environment)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    logger.LogError(ex,
                        "request {TraceId} failed after the response started;the response cannot be replaced",
                        context.TraceIdentifier);

                    throw;
                }

                logger.LogError(ex,
                   "unhandeld expetion while the processing request {TraceId}",
                   context.TraceIdentifier);

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;

                var problem = ExcptionProblemDetailsFactory.Create(ex, context, environment);

                await context.Response.WriteAsJsonAsync(
                    problem,
                    options:null,
                    contentType:"application/problem+json",
                    cancellationToken:context.RequestAborted);
            }
        }
    }
}
