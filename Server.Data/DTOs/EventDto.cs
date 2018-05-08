namespace Server.Data.DTOs
{
    public enum EventType
    {
        NoAction,
        Update,
        Insert,
        Delete
    }

    public enum RequestType
    {
        LobbyCreation,
        LobbyUpdate,
        LobbyJoinRequestCreation,
        LobbyJoinRequestAccept,
        LobbyJoinRequestDelete,
        MessageCreation
    }

    public class EventDto
    {
        public EventType EventType { get; set; }
        public string EventDescription { get; set; }
        public RequestType RequestType { get; set; }
        public string RequestDescription { get; set; }
        public object Data { get; set; }
    }
}
