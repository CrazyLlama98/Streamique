using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Data.DTOs;
using Server.Data.Interfaces;
using Server.Data.Models;
using Server.EventBus.Interfaces;
using Server.Services.Constants;
using Server.Services.IntegrationEvents.LobbyJoinRequestsEvents;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class LobbyJoinRequestService : GenericService<LobbyJoinRequest, LobbyJoinRequestDto>, ILobbyJoinRequestService
    {
        private readonly ILobbyRepository _lobbyRepository;
        private readonly IEventBus _eventBus;

        public LobbyJoinRequestService(
            ILobbyJoinRequestRepository repository, 
            ILobbyRepository lobbyRepository,
            IEventBus eventBus,
            IMapper mapper, 
            ILogger<LobbyJoinRequestService> logger) 
            : base(repository, mapper, logger)
        {
            _lobbyRepository = lobbyRepository;
            _eventBus = eventBus;
        }

        public bool AcceptLobbyJoinRequest(int id, int userId)
        {
            try
            {
                var existingLobbyJoinRequest = ((ILobbyJoinRequestRepository)Repository).GetLobbyJoinRequestLobbyInformation(id);
                if (existingLobbyJoinRequest == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.LobbyJoinRequestUpdateEntryNotFound, "Lobby join Request with id = {ID}, issued by user with id = {USERID} was not found", id, userId);
                    return false;
                }
                if (userId != existingLobbyJoinRequest.Lobby.HostId)
                    return false;
                existingLobbyJoinRequest.Accepted = true;
                Repository.Update(existingLobbyJoinRequest);
                Repository.SaveChanges();
                _eventBus.Publish(new AcceptJoinRequestEvent()
                {
                    AcceptedUserId = existingLobbyJoinRequest.UserId.Value,
                    LobbyId = existingLobbyJoinRequest.LobbyId.Value
                });
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyJoinRequestUpdateError, e, "Lobby join request with id = {ID}, issued by user with id = {USERID} cannot be accepted", id, userId);
                return false;
            }
        }

        public LobbyJoinRequestDto CreateLobbyJoinRequest(int lobbyId, int userId)
        {
            try
            {
                var newLobbyJoinRequest = new LobbyJoinRequest { Accepted = false, LobbyId = lobbyId, UserId = userId };
                var existingLobby = _lobbyRepository.Find(lobbyId);
                if (existingLobby == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.LobbyUpdateEntryNotFound, "Lobby with id = {ID} was not found", lobbyId);
                    return null;
                }
                if (existingLobby.HostId == userId)
                    return null;
                Repository.Add(newLobbyJoinRequest);
                Repository.SaveChanges();
                Logger.LogInformation(LoggingEvents.CustomServiceEvents.LobbyJoinRequestCreateInformation, "User with id = {USERID} created a join request for lobby with id = {LOBBYID}",
                    userId, lobbyId);
                _eventBus.Publish(new CreateJoinRequestEvent()
                {
                    Accepted = newLobbyJoinRequest.Accepted,
                    LobbyId = newLobbyJoinRequest.LobbyId.Value,
                    UserId = newLobbyJoinRequest.UserId.Value
                });
                return Mapper.Map<LobbyJoinRequestDto>(newLobbyJoinRequest);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyJoinRequestCreateError, e, "Failed to create lobby join request for user with id = {USERID} for lobby with id = {LOBBYID}", 
                    userId, lobbyId);
                return null;
            }
        }

        public bool DeleteLobbyJoinRequest(int id, int userId)
        {
            try
            {
                var existingLobbyJoinRequest = ((ILobbyJoinRequestRepository)Repository).GetLobbyJoinRequestLobbyInformation(id);
                if (existingLobbyJoinRequest == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.LobbyDeleteEntryNotFound, "Lobby join Request with id = {ID}, issued by user with id = {USERID} was not found", id, userId);
                    return false;
                }
                if (existingLobbyJoinRequest.UserId != userId && userId != existingLobbyJoinRequest.Lobby.HostId)
                    return false;
                var deleteJoinRequestEvent = new DeleteJoinRequestEvent()
                {
                    LobbyId = existingLobbyJoinRequest.LobbyId.Value,
                    UserId = existingLobbyJoinRequest.UserId.Value,
                    RemoveUserFromLobby = existingLobbyJoinRequest.Accepted
                };
                Repository.Delete(existingLobbyJoinRequest);
                Repository.SaveChanges();
                _eventBus.Publish(deleteJoinRequestEvent);
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyDeleteError, e, "Error deleting lobby join request with id = {ID}, issued by user {USERID} ", id, userId);
                return false;
            }
        }

        public IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByLobbyId(int id)
        {
            try
            {
                var lobbyRequests = ((ILobbyJoinRequestRepository)Repository).GetLobbyJoinRequestsByLobbyId(id);
                return Mapper.Map<IEnumerable<LobbyJoinRequestDto>>(lobbyRequests);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching the join requests for lobby with id = {ID}", id);
                return null;
            }
        }

        public IEnumerable<LobbyJoinRequestDto> GetLobbyJoinRequestsByUserId(int id)
        {
            try
            {
                var lobbyRequests = ((ILobbyJoinRequestRepository)Repository).GetLobbyJoinRequestsByUserId(id);
                return Mapper.Map<IEnumerable<LobbyJoinRequestDto>>(lobbyRequests);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching the join requests of user with id = {ID}", id);
                return new List<LobbyJoinRequestDto>();
            }
        }
    }
}
