using CITNASDaily.Entities.Dtos.UserDtos;

namespace CITNASDaily.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> CreateUserAsync(UserCreateDto userCreate);
        Task<bool> DoesUserExist(Guid userId);
        Task<bool> DoesUsernameExist(string username);
    }
}
