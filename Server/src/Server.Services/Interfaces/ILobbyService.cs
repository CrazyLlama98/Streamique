using System.Collections.Generic;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface ILobbyService : IGenericService<Lobby, LobbyDto>
    {
        IEnumerable<LobbyDto> GetLobbiesWithJoinRequests();
        LobbyDto GetLobbyWithJoinRequestById(int id);
        IEnumerable<LobbyDto> GetLobbiesByHostId(int hostId, bool includeRequests = false);
        LobbyDto CreateLobby(LobbyCreationDto newLobby, int userId);
        bool UpdateLobby(LobbyUpdateDto lobby, int lobbyId, int userId);
        bool DeleteLobby(int lobbyId, int userId);
    }
}
