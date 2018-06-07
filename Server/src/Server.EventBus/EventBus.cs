using Microsoft.Extensions.Logging;
using Server.EventBus.Events;
using Server.EventBus.Interfaces;

namespace Server.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _logger;
        private readonly IEventSubscriptionsManager _substriptionsManager;

        public EventBus(ILogger<EventBus> logger, IEventSubscriptionsManager substriptionsManager)
        {
            _logger = logger;
            _substriptionsManager = substriptionsManager;
        }

        public void Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            var eventHandlers = _substriptionsManager.GetEventHandlers<T>();
            if (eventHandlers != null)
            {
                foreach (var eventHandler in eventHandlers)
                {
                    eventHandler.OnHandle(integrationEvent);
                }
            }
        }

        public void Subscribe<T, TH>(TH integrationEventHandler)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _substriptionsManager.AddSubscription<T, TH>(integrationEventHandler);
        }

        public void Unsubscribe<T, TH>(TH integrationEventHandler)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _substriptionsManager.RemoveSubscription<T, TH>(integrationEventHandler);
        }
    }
}
