using System;
using Server.EventBus.Events;
using Server.EventBus.Interfaces;

namespace Server.EventBus
{
    public class IntegrationEventHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        public IntegrationEventHandler(Action<T> OnHandleCallback)
        {
            OnHandle = OnHandleCallback;
        }

        public Action<T> OnHandle { get; }
    }
}
