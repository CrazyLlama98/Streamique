using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Server.Data.DTOs;

namespace Server.Services.Interfaces
{
    public interface IAuthorizationService
    {
        string GenerateJwt();
        Task<IdentityResult> RegisterUserAsync(RegistrationDto registerDto);
        Task<SignInResult> LoginUserAsync(LoginDto loginDto);
    }
}
