using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Data.MappingsProfiles;
using Server.Data.Interfaces;
using Server.Data.Repositories;
using Server.Data.MappingProfiles;

namespace Server.Data
{
    public static class Extensions
    {
        public static IWebHost EnsureDatabaseCreated<T>(this IWebHost host) where T : DbContext 
        {
            var serviceScopeFactory = (IServiceScopeFactory) host.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<T>();

                dbContext.Database.EnsureCreated();
            }
            return host;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => {
                cfg.AddProfile(new UserMappingProfile());
                cfg.AddProfile(new LobbyMappingProfile());
                cfg.AddProfile(new MessageMappingProfile());
            });
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUserRepository), typeof(UserRepository))
                .AddTransient(typeof(ILobbyRepository), typeof(LobbyRepository))
                .AddTransient(typeof(ILobbyJoinRequestRepository), typeof(LobbyJoinRequestRepository))
                .AddTransient(typeof(IMessageRepository), typeof(MessageRepository));
            return services;
        }
    }
}
