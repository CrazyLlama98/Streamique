using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Data.DbContexts;
using Server.Data.Interfaces;
using Server.Data.Models;

namespace Server.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(VideoPanzerDbContext context)
            : base(context) { }

        public User FindByEmail(string Email)
        {
            return Context.Set<User>().FirstOrDefault(u => u.Email.Equals(Email, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
