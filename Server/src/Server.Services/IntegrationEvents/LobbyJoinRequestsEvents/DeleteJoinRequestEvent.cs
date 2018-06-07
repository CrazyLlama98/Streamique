using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyJoinRequestsEvents
{
    public class DeleteJoinRequestEvent : IntegrationEvent
    {
        public int LobbyId { get; set; }
        public int UserId { get; set; }
        public bool RemoveUserFromLobby { get; set; }
    }
}
