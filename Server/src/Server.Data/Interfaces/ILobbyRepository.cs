using System.Collections.Generic;
using Server.Data.Models;

namespace Server.Data.Interfaces
{
    public interface ILobbyRepository : IGenericRepository<Lobby>
    {
        IEnumerable<Lobby> GetLobbiesWithJoinRequests();
        Lobby GetLobbyWithJoinRequestById(int id);
        IEnumerable<Lobby> GetLobbiesByHostId(int hostId, bool includeRequests = false);
    }
}
