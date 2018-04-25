using System.Collections.Generic;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface ILobbyJoinRequestService : IGenericService<LobbyJoinRequest, LobbyJoinRequestDto>
    {
        IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByLobbyId(int id);
        IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByUserId(int id);
        LobbyJoinRequestDto CreateLobbyJoinRequest();
        bool DeleteLobbyJoinRequest(int id);
    }
}
