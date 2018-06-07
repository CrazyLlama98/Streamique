using System;
using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyJoinRequestsEvents
{
    public class CreateJoinRequestEvent : IntegrationEvent
    {
        public int LobbyId { get; set; }
        public int UserId { get; set; }
        public bool Accepted { get; set; }
    }
}
