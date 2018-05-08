using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Data.DTOs;
using Server.Data.Interfaces;
using Server.Data.Models;
using Server.Services.Constants;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class MessageService : GenericService<Message, MessageDto>, IMessageService
    {
        private ILobbyRepository _lobbyRepository;

        public MessageService(
            IMessageRepository repository, 
            ILobbyRepository lobbyRepository,
            IMapper mapper, 
            ILogger<MessageService> logger) 
            : base(repository, mapper, logger)
        {
            _lobbyRepository = lobbyRepository;
        }

        public MessageDto AddNewMessage(int lobbyId, int userId, MessageCreationDto message)
        {
            try
            {
                var existingLobby = _lobbyRepository.GetLobbyWithJoinRequestById(lobbyId);
                if (existingLobby == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.NewMessageAddError, "Lobby with id = {ID} was not found", lobbyId);
                    return null;
                }
                if (userId != existingLobby.HostId && existingLobby.JoinRequests.Where(u => u.UserId == userId).Count() == 0)
                    return null;
                var newMessage = Mapper.Map<Message>(message);
                newMessage.LobbyId = lobbyId;
                newMessage.SenderId = userId;
                Repository.Add(newMessage);
                Repository.SaveChanges();
                return Mapper.Map<MessageDto>(newMessage);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.NewMessageAddError, e,
                    "Error addind new message on the lobby with id = {LOBBYID}, by user with id = {USERID}", lobbyId, userId);
                return null;
            }
        }

        public IEnumerable<MessageDto> GetMessagesByLobbyId(int id, int? skip, int? take)
        {
            try
            {
                var messages = ((IMessageRepository)Repository).GetMessagesByLobbyId(id, skip, take);
                return Mapper.Map<IEnumerable<MessageDto>>(messages);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching messages of lobby with id = {ID}", id);
                return null;
            }
        }

        public IEnumerable<MessageDto> GetMessagesByUserId(int id, int? skip, int? take)
        {
            try
            {
                var messages = ((IMessageRepository)Repository).GetMessagesByUserId(id, skip, take);
                return Mapper.Map<IEnumerable<MessageDto>>(messages);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching messages of user with id = {ID}", id);
                return null;
            }
        }
    }
}
