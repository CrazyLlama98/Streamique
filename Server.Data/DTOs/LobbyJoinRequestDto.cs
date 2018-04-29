using System;

namespace Server.Data.DTOs
{
    public class LobbyJoinRequestDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Accepted { get; set; }
    }
}
