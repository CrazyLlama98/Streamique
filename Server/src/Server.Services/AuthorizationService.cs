﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private IHttpContextAccessor _accessor;
        private IConfiguration _configuration;
        private ILogger<AuthorizationService> _logger;
        private IMapper _mapper;

        public AuthorizationService(UserManager<User> userManager, RoleManager<UserRole> roleManager, IHttpContextAccessor accessor,
            IConfiguration configuration, ILogger<AuthorizationService> logger, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _accessor = accessor;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        public virtual string GenerateJwt(string email)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, _userManager.FindByEmailAsync(email).Result.Id.ToString()) };
                var token = new JwtSecurityToken(audience: _configuration["Jwt:Issuer"],
                                                    issuer: _configuration["Jwt:Issuer"],
                                                    claims: claims,
                                                    expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                                                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.JWTGenerationError, e, "JWT could not be generated");
                return null;
            }
        }

        public virtual async Task<IdentityResult> RegisterUserAsync(RegistrationDto registerDto)
        {
            try
            {
                User newUser = _mapper.Map<User>(registerDto);
                newUser.LastUsedIPAddress = _accessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return await _userManager.CreateAsync(newUser, registerDto.Password);
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.RegisterError, e, "Error registering user with email = {EMAIL}", registerDto.Email);
                return null;
            }
        }

        public virtual async Task<SignInResult> LoginUserAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (result.Succeeded)
                    await UpdateLastUsedIPAsync(user);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.LoginError, e, "Error on log in for user with email = {EMAIL}", loginDto.Email);
                return null;
            }
        }

        public async Task<bool> LogoffUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.AuthorizationEvents.LogoffError, e, "Error on log off");
                return false;
            }
        }

        public int GetUserId(ClaimsPrincipal User)
        {
            return int.Parse(_userManager.GetUserId(User));
        }

        private async Task UpdateLastUsedIPAsync(User user)
        {
            var ipAddress = _accessor?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4()?.ToString();
            if (!String.IsNullOrWhiteSpace(user.LastUsedIPAddress) && ! user.LastUsedIPAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase))
            {
                user.LastUsedIPAddress = ipAddress;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
