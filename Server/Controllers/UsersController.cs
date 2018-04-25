using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs;
using Server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll();
            if (users == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);
            return Ok(users);
        }

        [HttpGet("getByEmail/{email}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUserByEmail(string email)
        {
            if (email == null)
                return BadRequest();
            var user = _userService.FindByEmail(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUserById(int id)
        {
            if (id == 0)
                return BadRequest();
            var user = _userService.Find(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
