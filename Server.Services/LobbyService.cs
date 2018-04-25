using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Data.DTOs;
using Server.Data.Interfaces;
using Server.Data.Models;
using Server.Services.Constants;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class LobbyService : GenericService<Lobby, LobbyDto>, ILobbyService
    {
        public LobbyService(ILobbyRepository repository, IMapper mapper, ILogger<LobbyService> logger) 
            : base(repository, mapper, logger)
        { }

        public LobbyDto CreateLobby(LobbyCreationDto newLobby, int userId)
        {
            try
            {
                var lobbyModel = Mapper.Map<Lobby>(newLobby);
                lobbyModel.HostId = userId;
                Repository.Add(lobbyModel);
                Repository.SaveChanges();
                Logger.LogInformation(LoggingEvents.CustomServiceEvents.LobbyCreateInformation, "New Lobby Created for user with id = {ID}",
                    userId);
                return Mapper.Map<LobbyDto>(lobbyModel);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyCreateError, e, "Error Creating lobby for user with id = {ID}", 
                    userId);
                return null;
            }
        }

        public bool DeleteLobby(int lobbyId, int userId)
        {
            try
            {
                var existingLobby = Repository.Find(lobbyId);
                if (existingLobby == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.LobbyDeleteEntryNotFound, "Lobby with id = {ID} was not found", lobbyId);
                    return false;
                }
                if (existingLobby.HostId != userId)
                    return false;
                Repository.Delete(existingLobby);
                Repository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyDeleteError, e, "Error deleting lobby with id = {id}", lobbyId);
                return false;
            }
        }

        public IEnumerable<LobbyDto> GetLobbiesByHostId(int hostId, bool includeRequests = false)
        {
            try
            {
                var lobbies = ((ILobbyRepository)Repository).GetLobbiesByHostId(hostId, includeRequests);
                return Mapper.Map<IEnumerable<LobbyDto>>(lobbies);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching lobbies by host with id = {ID}!", hostId);
                return null;
            }
        }

        public IEnumerable<LobbyDto> GetLobbiesWithJoinRequests()
        {
            try
            {
                var lobbies = ((ILobbyRepository)Repository).GetLobbiesWithJoinRequests();
                return Mapper.Map<IEnumerable<LobbyDto>>(lobbies);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching lobbies!");
                return null;
            }
        }

        public LobbyDto GetLobbyWithJoinRequestById(int id)
        {
            try
            {
                var lobbies = ((ILobbyRepository)Repository).GetLobbyWithJoinRequestById(id);
                return Mapper.Map<LobbyDto>(lobbies);
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.GenericServiceEvents.LookupError, e, "Error fetching lobby with id = {ID}!", id);
                return null;
            }
        }

        public bool UpdateLobby(LobbyUpdateDto lobby, int lobbyId, int userId)
        {
            try
            {
                var existingLobby = Repository.Find(lobbyId);
                if (existingLobby == null)
                {
                    Logger.LogWarning(LoggingEvents.CustomServiceEvents.LobbyUpdateEntryNotFound, "Lobby with id = {ID} was not found", lobbyId);
                    return false;
                }
                if (existingLobby.HostId != userId)
                    return false;
                Repository.Update(Mapper.Map(lobby, existingLobby));
                Repository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(LoggingEvents.CustomServiceEvents.LobbyUpdateError, e, "Error updating lobby with id = {ID}", lobbyId);
                return false;
            }
        }
    }
}
