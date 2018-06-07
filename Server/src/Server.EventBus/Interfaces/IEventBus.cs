using Server.EventBus.Events;

namespace Server.EventBus.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T integrationEvent) where T : IntegrationEvent;

        void Subscribe<T, TH>(TH integrationEventHandler)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>(TH integrationEventHandler)
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
