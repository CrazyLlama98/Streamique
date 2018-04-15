using Microsoft.AspNetCore.Identity;

namespace Server.Data.Models
{
    public class User : IdentityUser<int>
    {
        public string Nickname { get; set; }
    }
}
