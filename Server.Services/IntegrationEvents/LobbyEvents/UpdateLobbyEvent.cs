using Server.Data.DTOs;
using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyEvents
{
    public class UpdateLobbyEvent : IntegrationEvent
    {
        public int UpdatedLobbyId { get; set; }
        public LobbyDto UpdatedLobby { get; set; }
    }
}
