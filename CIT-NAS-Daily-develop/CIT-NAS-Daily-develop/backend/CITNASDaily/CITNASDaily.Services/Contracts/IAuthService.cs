using CITNASDaily.Entities.Dtos.UserDtos;
using System.Security.Claims;

namespace CITNASDaily.Services.Contracts
{
    public interface IAuthService
    {
        Task<UserDto> RegisterUser(UserCreateDto userCreate);

        Task<string?> Login(UserLoginDto userLogin);

        UserDto? GetCurrentUser(ClaimsIdentity identity);
    }
}
