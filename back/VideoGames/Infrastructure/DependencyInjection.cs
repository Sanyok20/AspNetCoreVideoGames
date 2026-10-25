using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Text;
using VideoGames.BLL.Services;
using VideoGames.BLL.Tools;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<GameRepository>();
            services.AddScoped<DeveloperRepository>();
            services.AddScoped<GenreRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<UserTokenRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<GameService>();
            services.AddScoped<FileService>();
            services.AddScoped<GenreService>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            services.AddScoped<EmailService>();
            services.AddScoped<PasswordHasher<User>>();
            services.AddScoped<PaginateCollection>();

            return services;
        }

        public static IServiceCollection AddJobs(this IServiceCollection services, params (Type type, string cronExpression)[] jobs)
        {
            services.AddQuartz(q =>
            {
                foreach (var job in jobs)
                {
                    var jobKey = new JobKey(job.type.Name);
                    q.AddJob(job.type, jobKey);

                    q.AddTrigger(opt => opt
                        .ForJob(jobKey)
                        .WithIdentity($"{job.type.Name}-trigger")
                        .WithCronSchedule(job.cronExpression));
                }
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(optons =>
                {
                    optons.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]
                        ?? throw new ArgumentNullException("Jwt sercret key is null")))
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwaggerAuthorization(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "GamesShop", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Необхідно мати header Authorization: Bearer jwtToken"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}