using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]"), AllowAnonymous]
    public class AccountsController : Controller
    {
        private readonly Services.Interfaces.IAuthorizationService _authorizationService;

        public AccountsController(Services.Interfaces.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationService.LoginUserAsync(loginDto);
                if (!result.Succeeded)
                    return BadRequest();
                var token = _authorizationService.GenerateJwt(loginDto.Email);
                if (token != null)
                    return Ok(new { token });
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var resultRegister = await _authorizationService.RegisterUserAsync(registerDto);
                if (!resultRegister.Succeeded)
                    return BadRequest();
                var resultSignIn = await _authorizationService.LoginUserAsync(new LoginDto { Email = registerDto.Email, Password = registerDto.Password });
                if (!resultSignIn.Succeeded)
                    return BadRequest();
                var token = _authorizationService.GenerateJwt(registerDto.Email);
                if (token != null)
                    return Ok(new { token });
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return BadRequest();
        }

        [HttpPost("logoff")]
        public async Task<IActionResult> Logoff()
        {
            if (await _authorizationService.LogoffUserAsync())
                return Ok();
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
