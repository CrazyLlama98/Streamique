using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface IUserService : IGenericService<User, UserDto>
    {
        UserDto FindByEmail(string email);
    }
}
