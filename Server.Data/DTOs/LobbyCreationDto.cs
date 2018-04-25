using System.ComponentModel.DataAnnotations;

namespace Server.Data.DTOs
{
    public class LobbyCreationDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
