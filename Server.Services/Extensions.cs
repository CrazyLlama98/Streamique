using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Server.Data.DbContexts;
using Server.Data.Models;
using Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IAuthorizationService), typeof(AuthorizationService))
                .AddTransient(typeof(IUserService), typeof(UserService))
                .AddTransient(typeof(ILobbyService), typeof(LobbyService))
                .AddTransient(typeof(ILobbyJoinRequestService), typeof(LobbyJoinRequestService))
                .AddTransient(typeof(IMessageService), typeof(MessageService));
            return services;
        }

        public static IServiceCollection AddUserDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<VideoPanzerDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddIdentity<User, UserRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<VideoPanzerDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthenticationOptions(this IServiceCollection services, string validIssuer, string key)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = validIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    };
                    options.RequireHttpsMetadata = false;
                });
            return services;
        }
    }
}
