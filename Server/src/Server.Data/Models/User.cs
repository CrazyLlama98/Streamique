using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Server.Data.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string AccountImageUrl { get; set; }
        public string LastUsedIPAddress { get; set; }
        public IEnumerable<Lobby> Lobbies { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
