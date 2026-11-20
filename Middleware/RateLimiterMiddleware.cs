using E_commerce_System.Data;

namespace E_commerce_System.Middleware
{
    public class RateLimiterMiddleware(RequestDelegate next)
    {
        private static int _counter = 0;
        private static DateTime _lastRequestDate = DateTime.Now;


        public async Task InvokeAsync(HttpContext context)
        {
            _counter++;

            if(DateTime.Now.Subtract(_lastRequestDate).Seconds>10)
            {
                _counter++;
                _lastRequestDate = DateTime.Now;
                await next(context);

            }
            else
            {
                if(_counter >5)
                {
                    _lastRequestDate = DateTime.Now;
                    await context.Response.WriteAsync("rate limit exceded");

                }
                else
                {
                    _lastRequestDate = DateTime.Now;
                    await next(context);
                }
            }
        }
    }
}
