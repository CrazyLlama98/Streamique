using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Server.Data.DTOs;
using Server.Data.Models;
using Server.Services.Constants;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        private SignInManager<User> _signInManager;
        private IConfiguration _configuration;
        private ILogger<AuthorizationService> _logger;

        public AuthorizationService(UserManager<User> userManager, RoleManager<UserRole> roleManager, 
            IConfiguration configuration, ILogger<AuthorizationService> logger, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwt()
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                                                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                                                signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            } catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.JWTGenerationError, e, "JWT could not be generated");
                return null;
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(RegistrationDto registerDto)
        {
            try
            {
                User newUser = new User { Nickname = registerDto.NickName, Email = registerDto.Email, UserName = registerDto.Email };
                return await _userManager.CreateAsync(newUser, registerDto.Password);
            } catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.RegisterError, e, "Error registering user with email = {EMAIL}", registerDto.Email);
                return null;
            }
        }

        public async Task<SignInResult> LoginUserAsync(LoginDto loginDto)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.LoginError, e, "Error on log in for user with email = {EMAIL}", loginDto.Email);
                return null;
            }
        }
    }
}
