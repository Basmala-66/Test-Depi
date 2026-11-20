using E_commerce_System.Data;
using E_commerce_System.Entities;
using E_commerce_System.filters;
using E_commerce_System.Helper;
using E_commerce_System.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_commerce_System.Extentions
{
    public static class ApplicationServiceExctention
    {
        public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            // register dbcontext as a scoped service os inversion of contorl (IOC)
           services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"))
            );


            //register auto mapper
            services.AddAutoMapper(cof =>
            {
                cof.AddProfile<ProductProfile>();
                cof.AddProfile<CategoryProfile>();
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<ExcptionHandlingFilter>();
                options.Filters.Add<ActionExecutionLogingFilter>();
                options.Filters.Add<ActionTimingFilter>();
            });

            var corsOrigin = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()?? Array.Empty<string>();
            const string corsname = "ConfigurationOrigin";
            services.AddCors(op=>
            op.AddPolicy(corsname, policy =>
            {
                if(corsOrigin.Length > 0)
                {
                    policy.WithOrigins(corsOrigin);
                }

                policy.AllowAnyHeader().AllowAnyMethod();
            }));


            // Register database contexts.
           
            services.AddDbContext<AuthnticationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("AuthConectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthnticationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op=>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),


                };
            });

            services.AddScoped<IJwtService, JwtService>();
            
            return services;

        }
    }
}
