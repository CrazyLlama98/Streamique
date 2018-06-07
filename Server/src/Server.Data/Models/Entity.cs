using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
