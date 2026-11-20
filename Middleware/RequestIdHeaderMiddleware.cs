
namespace E_commerce_System.Middleware
{
    /// <summary>
    /// Middleware that adds a unique request identifier to the response headers and logs the assignment for tracking
    /// and debugging purposes.
    /// </summary>
    /// <param name="logger">The logger used to record debug information about request ID assignment.</param>
    public sealed class RequestIdIdMiddleware(ILogger<RequestIdIdMiddleware>logger) : IMiddleware
    {

        //IMiddleware activitetd throw ID for each request
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var reaquestId = context.TraceIdentifier;
            context.Response.Headers["X-Request-Id"] = reaquestId;

            logger.LogDebug("assigment request id {RequestId} to {Method} {Path}",
                reaquestId,
                context.Request.Method,
                context.Request.Path);

            await next(context);
        }
    }
}
