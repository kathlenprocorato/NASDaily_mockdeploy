using AutoMapper;
using CITNASDaily.Entities.Dtos.OASDtos;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Services.Contracts;
using CITNASDaily.Utils;

namespace CITNASDaily.Services.Services
{
    public class SuperiorService : ISuperiorService
    {
        private readonly ISuperiorRepository _superiorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SuperiorService(ISuperiorRepository superiorRepository, IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _superiorRepository = superiorRepository;
        }
        public async Task<SuperiorDto?> CreateSuperiorAsync(string username, SuperiorCreateDto superiorCreate)
        {
            var superior = _mapper.Map<Superior>(superiorCreate);

            var userId = await GetSuperiorUserIdByUsernameAsync(username);

            if (userId == null)
            {
                // subject to change
                return null;
            }

            superior.UserId = userId;
            var createdSuperior = await _superiorRepository.CreateSuperiorAsync(superior);

            return _mapper.Map<SuperiorDto>(createdSuperior);
        }


        public async Task<SuperiorDto?> GetSuperiorAsync(int superiorId)
        {
            var superior = await _superiorRepository.GetSuperiorAsync(superiorId);
            return _mapper.Map<SuperiorDto>(superior);
        }

        public async Task<SuperiorDto?> GetSuperiorByOfficeId(int officeId)
        {
            var superior = await _superiorRepository.GetSuperiorByOfficeId(officeId);
            return _mapper.Map<SuperiorDto>(superior);
        }

        public async Task<IEnumerable<SuperiorDto>> GetSuperiorsAsync()
        {
            var superior = await _superiorRepository.GetSuperiorsAsync();
            return _mapper.Map<IEnumerable<SuperiorDto>>(superior);
        }

        public async Task<Guid?> GetSuperiorUserIdByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public async Task<int> GetSuperiorIdByUsernameAsync(string username)
        {
            return await _superiorRepository.GetSuperiorIdByUsernameAsync(username);
        }

        public async Task<bool> ChangePasswordAsync(int superiorId, string currentPassword, string newPassword)
        {
            var superior = await _superiorRepository.GetSuperiorAsync(superiorId);

            //nas does not exist
            if (superior == null)
            {
                return false;
            }

            var user = await _userRepository.GetUserByUsernameAsync(superior.Username);

            bool check = PasswordManager.VerifyPassword(currentPassword, user.PasswordHash);

            //password do not match
            if (check == false)
            {
                return false;
            }

            return await _superiorRepository.ChangePasswordAsync(superiorId, PasswordManager.HashPassword(newPassword));
        }
    }
}
