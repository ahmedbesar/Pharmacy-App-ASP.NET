using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Domian.IdentityDtos;
namespace Pharmacy.Domian.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto model);
        Task<AuthDto> GetTokenAsync(TokenRequestDto model);
        Task<string> AddRoleAsync(AddRoleDto model);
        Task<AuthDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> UpdateUser(string token, UpdateUserDto dto);
        Task<ApplicationUser> GetCurrentUserById(string userName);
    }
}