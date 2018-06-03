using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyJoinRequestsEvents
{
    public class AcceptJoinRequestEvent : IntegrationEvent
    {
        public int LobbyId { get; set; }
        public int AcceptedUserId { get; set; }
    }
}
