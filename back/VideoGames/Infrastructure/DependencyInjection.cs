using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VideoGames.BLL.Services;
using System.Text;
using VideoGames.BLL.Tools;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;
using VideoGames.BLL.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
    }
}