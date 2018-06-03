using System;
using System.Collections.Generic;
using Server.EventBus.Events;

namespace Server.EventBus.Interfaces
{
    public interface IEventSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void AddSubscription<T, TH>(TH integrationEventHandler)
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;
        void RemoveSubscription<T, TH>(TH integrationEventHandler)
             where TH : IIntegrationEventHandler<T>
             where T : IntegrationEvent;
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        IEnumerable<IIntegrationEventHandler<T>> GetEventHandlers<T>() where T : IntegrationEvent;
        Type GetEventTypeByName(string eventName);
        void Clear();
        string GetEventKey<T>();
    }
}
