using System;

namespace Server.Data.Models
{
    public class LobbyJoinRequest : Entity
    {
        public User User { get; set; }
        public Lobby Lobby { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Accepted { get; set; }
        public int? LobbyId { get; set; }
        public int? UserId { get; set; }
    }
}
