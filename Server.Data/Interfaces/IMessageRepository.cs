using System.Collections.Generic;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Data.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        IEnumerable<Message> GetMessagesByLobbyId(int lobbyId, int? skip = null, int? take = null);
        IEnumerable<Message> GetMessagesByUserId(int userId, int? skip = null, int? take = null);
    }
}
