using Microsoft.Extensions.DependencyInjection;
using Server.EventBus.Interfaces;

namespace Server.EventBus
{
    public static class Extensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services
                .AddSingleton<IEventSubscriptionsManager, InMemoryEventSubscriptionsManager>()
                .AddSingleton<IEventBus, EventBus>();
            return services;
        }
    }
}
