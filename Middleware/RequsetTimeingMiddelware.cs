using System.Diagnostics;

namespace E_commerce_System.Middleware
{
    /// <summary>
    /// Middleware that measures and logs the time taken to process HTTP requests, including error handling.
    /// </summary>
    /// <param name="next">The delegate representing the next middleware in the request pipeline.</param>
    /// <param name="logger">The logger used to record request timing and error information.</param>
    public sealed class RequsetTimeingMiddelware(RequestDelegate next,ILogger<RequsetTimeingMiddelware>logger)
    {

        // use this style for use pipline behavior
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            Exception? failure = null;

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                failure = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                if(failure is null)
                {
                    logger.LogInformation("http {Method} {Path} returned {StatusCode} in {ElapesdMillisecinds} ms",
                      context.Request.Method,
                      context.Request.Path,
                      context.Response.StatusCode,
                      stopwatch.Elapsed.Microseconds);
                }
                else
                {
                    logger.LogError(
                        failure,
                        "HTTP {Method} {Path} Faliar after {ElapesdMillisecinds} ms",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.Elapsed.Microseconds
                        );
                }
            }
        }
    }
}
