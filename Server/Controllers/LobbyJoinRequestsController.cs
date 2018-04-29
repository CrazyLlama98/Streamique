using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs;
using Server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/lobbies/{lobbyId}/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LobbyJoinRequestsController : Controller
    {
        private ILobbyJoinRequestService _lobbyJoinRequestService;
        private Services.Interfaces.IAuthorizationService _authorizationService;

        public LobbyJoinRequestsController(
            ILobbyJoinRequestService lobbyJoinRequestService, 
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _lobbyJoinRequestService = lobbyJoinRequestService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LobbyJoinRequestDto>))]
        public IActionResult GetJoinRequests(int lobbyId)
        {
            if (lobbyId == 0)
                return BadRequest();
            return Ok(_lobbyJoinRequestService.GetLobbyJoinRequestsByLobbyId(lobbyId));
        }

        [HttpGet("{id}", Name = "GetLobbyJoinRequest")]
        [ProducesResponseType(200, Type = typeof(LobbyJoinRequestDto))]
        public IActionResult GetJoinRequest(int lobbyId, int id)
        {
            if (lobbyId == 0 || id == 0)
                return BadRequest();
            var lobbyRequest = _lobbyJoinRequestService.Find(id);
            if (lobbyRequest == null)
                return NotFound();
            return Ok(lobbyRequest);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(LobbyJoinRequestDto))]
        public IActionResult CreateLobbyJoinRequest(int lobbyId)
        {
            if (lobbyId == 0)
                return BadRequest();
            try
            {
                var userId = _authorizationService.GetUserId(User);
                var lobbyJoinRequestResult = _lobbyJoinRequestService.CreateLobbyJoinRequest(lobbyId, userId);
                if (lobbyJoinRequestResult != null)
                {
                    return CreatedAtRoute("GetLobbyJoinRequest", new { id = lobbyJoinRequestResult.Id, lobbyId }, lobbyJoinRequestResult);
                }
                return BadRequest();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}/accept")]
        [ProducesResponseType(204)]
        public IActionResult AcceptLobbyJoinRequest(int lobbyId, int id)
        {
            if (lobbyId == 0 || id == 0)
                return BadRequest();
            try
            {
                var userId = _authorizationService.GetUserId(User);
                var result = _lobbyJoinRequestService.AcceptLobbyJoinRequest(id, userId);
                if (result)
                    return NoContent();
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteLobbyJoinRequest(int lobbyId, int id)
        {
            if (lobbyId == 0 || id == 0)
                return BadRequest();
            try
            {
                var userId = _authorizationService.GetUserId(User); ;
                if (_lobbyJoinRequestService.DeleteLobbyJoinRequest(id, userId))
                    return NoContent();
            }
            catch { }
            return Unauthorized();
        }
    }
}
