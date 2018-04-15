using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Models;

namespace Server.Data.DbContexts
{
    public class UserDbContext : IdentityDbContext<User, UserRole, int>
    {
        public UserDbContext(DbContextOptions options) 
            : base(options)
        { }
    }
}
