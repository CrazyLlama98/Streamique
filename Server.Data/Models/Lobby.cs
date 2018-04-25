using System;
using System.Collections.Generic;

namespace Server.Data.Models
{
    public class Lobby : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public User Host { get; set; }
        public int? HostId { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<LobbyJoinRequest> JoinRequests { get; set; }
    }
}
