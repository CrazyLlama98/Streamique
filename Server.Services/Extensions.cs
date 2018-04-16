using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Server.Services.Interfaces;

namespace Server.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IAuthorizationService), typeof(AuthorizationService));
            return services;
        }
    }
}
