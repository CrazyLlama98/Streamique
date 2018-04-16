using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Server.Data.DbContexts;
using System;
using Microsoft.EntityFrameworkCore;

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
    }
}
