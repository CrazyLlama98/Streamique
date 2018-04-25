using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public LobbiesController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
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
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var lobbyResult = _lobbyService.CreateLobby(newLobby, userId);
                    if (lobbyResult != null)
                    {
                        return CreatedAtRoute("GetLobby", new { id = lobbyResult.Id }, lobbyResult);
                    }
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
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (_lobbyService.UpdateLobby(lobby, id, userId))
                    return NoContent();
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteLobby(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (_lobbyService.DeleteLobby(id, userId))
                return NoContent();
            return Unauthorized();
        }
    }
}
