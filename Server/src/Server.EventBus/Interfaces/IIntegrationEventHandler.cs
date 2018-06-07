using System;
using System.Threading.Tasks;
using Server.EventBus.Events;

namespace Server.EventBus.Interfaces
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Action<TIntegrationEvent> OnHandle { get; }
    }

    public interface IIntegrationEventHandler
    {

    }
}
