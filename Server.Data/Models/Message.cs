namespace Server.Data.Models
{
    public class Message : Entity
    {
        public int SenderId { get; set; }
        public int LobbyId { get; set; }
        public User Sender { get; set; }
        public Lobby Lobby { get; set; }
        public string Content { get; set; }
    }
}
