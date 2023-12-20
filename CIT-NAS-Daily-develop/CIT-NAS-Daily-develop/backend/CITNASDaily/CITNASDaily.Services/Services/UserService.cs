using AutoMapper;
using CITNASDaily.Entities.Dtos.UserDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Services.Contracts;

namespace CITNASDaily.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDto?> CreateUserAsync(UserCreateDto userCreate)
        {
            //subject to change
            //checking for role validation
            if (userCreate.Role == "OAS" ||  userCreate.Role == "NAS" || userCreate.Role == "Superior")
            {
                var user = _mapper.Map<User>(userCreate);
                var createdUser = await _userRepository.CreateUserAsync(user);

                return _mapper.Map<UserDto>(createdUser);
            }
            return null;
        }

        public async Task<bool> DoesUserExist(Guid userId)
        {
            return await _userRepository.DoesUserExist(userId);
        }

        public async Task<bool> DoesUsernameExist(string username)
        {
            return await _userRepository.DoesUsernameExist(username);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
