using System;
using System.Collections.Generic;
using System.Linq;
using Server.EventBus.Events;
using Server.EventBus.Interfaces;

namespace Server.EventBus
{
    public class InMemoryEventSubscriptionsManager : IEventSubscriptionsManager
    {

        private readonly Dictionary<string, List<IIntegrationEventHandler>> _handlers;
        private readonly List<Type> _eventTypes;
        private static readonly object SubscriptionsLock = new object();

        public InMemoryEventSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<IIntegrationEventHandler>>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => _handlers.Keys.Any();

        public event EventHandler<string> OnEventRemoved;

        public void AddSubscription<T, TH>(TH integrationEventHandler)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            string eventName = GetEventKey<T>();
            if (!HasSubscriptionsForEvent(eventName))
            {
                lock (SubscriptionsLock)
                {
                    _handlers.Add(eventName, new List<IIntegrationEventHandler>());
                }
            }
            lock (SubscriptionsLock)
            {
                _handlers[eventName].Add(integrationEventHandler);
            }
        }

        public void Clear()
        {
            lock (SubscriptionsLock)
            {
                _handlers.Clear();
            }
        }

        public string GetEventKey<T>() => typeof(T).Name;

        public Type GetEventTypeByName(string eventName)
        {
            lock (SubscriptionsLock)
            {
                return _eventTypes.FirstOrDefault(e => e.Name.Equals(eventName));
            }
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            lock (SubscriptionsLock)
            {
                return _handlers.ContainsKey(GetEventKey<T>());
            }
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            lock (SubscriptionsLock)
            {
                return _handlers.ContainsKey(eventName);
            }
        }

        public void RemoveSubscription<T, TH>(TH integrationEventHandler)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            lock (SubscriptionsLock)
            {
                string eventName = GetEventKey<T>();
                _handlers[eventName].Remove(integrationEventHandler);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name.Equals(eventName));
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            if (handler != null)
            {
                OnEventRemoved(this, eventName);
            }
        }

        public IEnumerable<IIntegrationEventHandler<T>> GetEventHandlers<T>() where T : IntegrationEvent
        {
            if (!HasSubscriptionsForEvent<T>())
                return null;
            string eventName = GetEventKey<T>();
            lock (SubscriptionsLock)
            {
                return _handlers[eventName].OfType<IIntegrationEventHandler<T>>().AsEnumerable();
            }
        }
    }
}
