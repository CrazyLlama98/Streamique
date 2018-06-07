using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Data.DTOs;
using Server.Data.MappingProfiles;
using Server.Data.Models;
using Server.Services;
using Xunit;

namespace Server.UnitTests.Accounts
{
    public class AccountsControllerTest
    {
        private readonly Mock<AuthorizationService> _authorizationService;

        public AccountsControllerTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserMappingProfile());
                cfg.AddProfile(new LobbyMappingProfile());
                cfg.AddProfile(new MessageMappingProfile());
            });
            var userManager = MockHelpers.MockUserManager<User>();
            _authorizationService = new Mock<AuthorizationService>(userManager.Object,
                MockHelpers.MockRoleManager<UserRole>().Object, new Mock<IHttpContextAccessor>().Object, MockHelpers.TestConfiguration(),
                MockHelpers.MockILogger<AuthorizationService>().Object, MockHelpers.TestSignInManager(userManager.Object), 
                Mapper.Configuration.CreateMapper());
        }

        [Fact]
        public void RegisterValidUser()
        {
            _authorizationService.Setup(x => x.RegisterUserAsync(It.IsAny<RegistrationDto>())).ReturnsAsync(IdentityResult.Success);
            _authorizationService.Setup(x => x.LoginUserAsync(It.IsAny<LoginDto>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _authorizationService.Setup(x => x.GenerateJwt(It.IsAny<string>())).Returns(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken()));
            var result = new AccountsController(_authorizationService.Object)
                .Register(new RegistrationDto {
                    AccountImageId = 4,
                    Email = "test-unit@test.com",
                    NickName = "testunit",
                    Password = "testunit",
                    Phone = "0766789123" })
                .Result;
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        }

        [Fact]
        public void RegisterInvalidUser()
        {
            _authorizationService.Setup(x => x.RegisterUserAsync(It.IsAny<RegistrationDto>())).ReturnsAsync<RegistrationDto, AuthorizationService, IdentityResult>(x => x.Email != null && x.Password != null ? IdentityResult.Success : IdentityResult.Failed());
            _authorizationService.Setup(x => x.LoginUserAsync(It.IsAny<LoginDto>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _authorizationService.Setup(x => x.GenerateJwt(It.IsAny<string>())).Returns(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken()));
            var result = new AccountsController(_authorizationService.Object)
                .Register(new RegistrationDto
                {
                    AccountImageId = 4,
                    NickName = "testunit",
                    Password = "testunit",
                    Phone = "0766789123"
                })
                .Result;
            var okResult = result.Should().BeOfType<BadRequestResult>().Subject;
        }
    }
}
