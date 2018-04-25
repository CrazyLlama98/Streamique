using System.ComponentModel.DataAnnotations;

namespace Server.Data.DTOs
{
    public class RegistrationDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NickName { get; set; }
        [StringLength(10, MinimumLength = 10), Required]
        public string Phone { get; set; }
        [Required]
        public int AccountImageId { get; set; }
    }
}
