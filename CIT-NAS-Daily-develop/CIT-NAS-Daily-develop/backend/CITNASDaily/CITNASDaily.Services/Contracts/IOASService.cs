using CITNASDaily.Entities.Dtos.OASDtos;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Services.Contracts
{
    public interface IOASService
    {
        Task<OASDto?> CreateOASAsync(string username, OASCreateDto oasCreate);
        public Task<OASDto?> GetOASAsync(int oasId);
        Task<Guid?> GetOASUserIdByUsernameAsync(string username);
        Task<int> GetOASIdByUsernameAsync(string username);
        Task<IEnumerable<OAS>?> GetAllOASAsync();
    }
}
