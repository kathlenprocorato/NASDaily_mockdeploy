using CITNASDaily.Entities.Dtos.UserDtos;

namespace CITNASDaily.Services.Contracts
{
    public interface ITokenService
    {
        /// <summary>
        /// Creates a token for the user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>The token for the user</returns>
        string CreateToken(UserDto userDto);
    }
}
