using System.ComponentModel.DataAnnotations;

namespace Server.Data.DTOs
{
    public class LobbyUpdateDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
