using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs;
using Server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LobbiesController : Controller
    {
        private ILobbyService _lobbyService;
        private Services.Interfaces.IAuthorizationService _authorizationService;

        public LobbiesController(
            ILobbyService lobbyService,
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _lobbyService = lobbyService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LobbyDto>))]
        public IActionResult GetLobbies(int? hostId)
        {
            if (hostId.HasValue)
            {
                return Ok(_lobbyService.GetLobbiesByHostId(hostId.Value, true));
            }
            return Ok(_lobbyService.GetAll());
        }

        [HttpGet("{id:int}", Name = "GetLobby")]
        [ProducesResponseType(200, Type = typeof(LobbyDto))]
        public IActionResult GetLobbiesById(int id)
        {
            if (id == 0)
                return BadRequest();
            return Ok(_lobbyService.GetLobbyWithJoinRequestById(id));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(LobbyDto))]
        public IActionResult CreateLobby([FromBody] LobbyCreationDto newLobby)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var userId = _authorizationService.GetUserId(User);
                        var lobbyResult = _lobbyService.CreateLobby(newLobby, userId);
                        if (lobbyResult != null)
                            return CreatedAtRoute("GetLobby", new { id = lobbyResult.Id }, lobbyResult);
                    }
                    catch
                    { }
                    return Unauthorized();
                }
                return BadRequest();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public IActionResult UpdateLobby(int id, [FromBody] LobbyUpdateDto lobby)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _authorizationService.GetUserId(User);
                    if (_lobbyService.UpdateLobby(lobby, id, userId))
                        return NoContent();
                }
                catch { }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteLobby(int id)
        {
            try
            {
                var userId = _authorizationService.GetUserId(User);
                if (_lobbyService.DeleteLobby(id, userId))
                    return NoContent();
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
