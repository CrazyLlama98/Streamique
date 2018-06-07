using Server.Data.Models;

namespace Server.Data.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User FindByEmail(string Email);
    }
}
