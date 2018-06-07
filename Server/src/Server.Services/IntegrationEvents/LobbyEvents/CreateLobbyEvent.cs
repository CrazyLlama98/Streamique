using Server.Data.DTOs;
using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyEvents
{
    public class CreateLobbyEvent : IntegrationEvent
    {
        public LobbyDto NewLobby { get; set; }
    }
}
