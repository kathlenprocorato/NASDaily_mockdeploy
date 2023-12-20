using AutoMapper;
using CITNASDaily.Entities.Dtos.UserDtos;
using CITNASDaily.Services.Contracts;
using CITNASDaily.Utils;
using System.Security.Claims;

namespace CITNASDaily.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _tokenService = tokenService;
        }

        public UserDto? GetCurrentUser(ClaimsIdentity identity)
        {
            if (identity == null) return null;

            return new UserDto
            {
                Id = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!),
                Username = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value!,
                Role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value!,
            };
        }

        public async Task<string?> Login(UserLoginDto userLogin)
        {
            var user = await _userService.GetUserByUsernameAsync(userLogin.Username);
            if (user == null) return null;
            if (!PasswordManager.VerifyPassword(userLogin.Password, user.PasswordHash)) return null;

            var userDto = _mapper.Map<UserDto>(user);

            return _tokenService.CreateToken(userDto);
        }

        public async Task<UserDto> RegisterUser(UserCreateDto userCreate)
        {
            userCreate.Password = PasswordManager.HashPassword(userCreate.Password);
            var createdUser = await _userService.CreateUserAsync(userCreate);

            return _mapper.Map<UserDto>(createdUser);
        }
    }
}
