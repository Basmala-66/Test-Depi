
using E_commerce_System.Data;
using E_commerce_System.Extentions;
using E_commerce_System.Helper;
using E_commerce_System.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace E_commerce_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            builder.Services.AddServices(builder.Configuration);
            
            var app = builder.Build();

            // automtic migration
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerfactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                using var context = services.GetRequiredService<AppDbContext>();
                await context.Database.MigrateAsync();
                await StorecontextSeeding.SeedAsync(context);
            }
            catch(Exception ex) 
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, ex.Message);
            }


            app.UseMiddleware<ExcepstionHandlingMiddleware>();
            app.UseMiddleware<RequsetTimeingMiddelware>();
            // app.UseMiddleware<RequestIdIdMiddleware>();
            //app.UseMiddleware<RateLimiterMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.Run(async context=>
            //{
            //   await context.Response.WriteAsync("short-circt Middeleware");
            //});

            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("work that can write to the response.(1)");
            //    await next.Invoke(context);
            //    Console.WriteLine("work that does not write to response (1)  ");
            //});

            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("work that can write to the response.(2)");
            //    await next.Invoke(context);
            //    Console.WriteLine("work that does not write to response (2)  ");
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("hello world");
            //});


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
