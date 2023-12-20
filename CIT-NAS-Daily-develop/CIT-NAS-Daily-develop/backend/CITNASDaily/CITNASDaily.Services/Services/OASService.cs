using AutoMapper;
using CITNASDaily.Entities.Dtos.OASDtos;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;

namespace CITNASDaily.Services.Services
{
    public class OASService : IOASService
    {
        private readonly IOASRepository _oasRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OASService(IOASRepository oasRepository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _oasRepository = oasRepository;
        }

        public async Task<OASDto?> CreateOASAsync(string username, OASCreateDto oasCreate)
        {
            var oas = _mapper.Map<OAS>(oasCreate);

            var userId = await GetOASUserIdByUsernameAsync(username);

            if (userId == null)
            {
                // subject to change
                return null;
            }

            oas.UserId = userId;
            var createdOAS = await _oasRepository.CreateOASAsync(oas);

            return _mapper.Map<OASDto>(createdOAS);
        }

        public Task<IEnumerable<OAS>?> GetAllOASAsync()
        {
            return _oasRepository.GetAllOASAsync();
        }

        public async Task<OASDto?> GetOASAsync(int oasId)
        {
            var oas = await _oasRepository.GetOAS(oasId);

            return _mapper.Map<OASDto>(oas);
        }

        public async Task<Guid?> GetOASUserIdByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public async Task<int> GetOASIdByUsernameAsync(string username)
        {
            return await _oasRepository.GetOASIdByUsernameAsync(username);
        }
    }
}
