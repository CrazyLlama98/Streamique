using System.Collections.Generic;
using Server.Data.Models;

namespace Server.Data.Interfaces
{
    public interface ILobbyJoinRequestRepository : IGenericRepository<LobbyJoinRequest>
    {
        IEnumerable<LobbyJoinRequest> GetLobbyJoinRequestsByLobbyId(int id);
        IEnumerable<LobbyJoinRequest> GetLobbyJoinRequestsByUserId(int id);
        LobbyJoinRequest GetLobbyJoinRequestLobbyInformation(int id);
    }
}
