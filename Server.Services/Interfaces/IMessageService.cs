using System.Collections.Generic;
using Server.Data.DTOs;
using Server.Data.Models;

namespace Server.Services.Interfaces
{
    public interface IMessageService : IGenericService<Message, MessageDto>
    {
        IEnumerable<MessageDto> GetMessagesByLobbyId(int id, int? skip, int? take);
        IEnumerable<MessageDto> GetMessagesByUserId(int id, int? skip, int? take);
        MessageDto AddNewMessage(int lobbyId, int userId, MessageCreationDto message);
    }
}
