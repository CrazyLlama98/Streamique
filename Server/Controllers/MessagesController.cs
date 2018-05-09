using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs;
using Server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/lobbies/{lobbyId}/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        private Services.Interfaces.IAuthorizationService _authorizationService;

        public MessagesController(
            IMessageService messageService,
            Services.Interfaces.IAuthorizationService authorizationService)
        {
            _messageService = messageService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult GetMessagesByLobbyId(int? lobbyId, int? page = null, int? pageSize = null)
        {
            if (!lobbyId.HasValue)
                return BadRequest();
            var messages = _messageService.GetMessagesByLobbyId(lobbyId.Value,
                pageSize.HasValue && page.HasValue ? pageSize * (page - 1) : null, pageSize);
            return Ok(messages);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult GetMessage(int? lobbyId, int? id)
        {
            if (!lobbyId.HasValue || !id.HasValue)
                return BadRequest();
            var message = _messageService.Find(id.Value);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public IActionResult AddNewMessage(int? lobbyId, [FromBody] MessageCreationDto message)
        {
            if (!lobbyId.HasValue)
                return BadRequest();
            int userId = _authorizationService.GetUserId(User);
            var newMessage = _messageService.AddNewMessage(lobbyId.Value, userId, message);
            if (newMessage == null)
                return Unauthorized();
            return CreatedAtRoute("GetMessage", new { lobbyId, id = newMessage.Id }, newMessage);
        }
    }
}
