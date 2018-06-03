using Server.EventBus.Events;

namespace Server.Services.IntegrationEvents.LobbyEvents
{
    public class DeleteLobbyEvent : IntegrationEvent
    {
        public int DeletedLobbyId { get; set; }
    }
}
