using System.Collections.Generic;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface ILobbyJoinRequestService : IGenericService<LobbyJoinRequest, LobbyJoinRequestDto>
    {
        IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByLobbyId(int id);
        IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByUserId(int id);
        LobbyJoinRequestDto CreateLobbyJoinRequest(int lobbyId, int userId);
        bool DeleteLobbyJoinRequest(int id, int userId);
        bool AcceptLobbyJoinRequest(int id, int userId);
    }
}
