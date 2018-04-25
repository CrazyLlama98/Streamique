using System;
using System.Collections.Generic;

namespace Server.Data.DTOs
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserDto Host { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<LobbyJoinRequestDto> JoinRequests { get; set; }
    }
}
