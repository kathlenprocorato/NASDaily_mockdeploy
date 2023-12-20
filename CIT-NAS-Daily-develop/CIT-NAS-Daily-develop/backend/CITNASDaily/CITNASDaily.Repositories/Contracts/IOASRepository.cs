using CITNASDaily.Entities.Models;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IOASRepository
    {
        Task<OAS?> CreateOASAsync(OAS oas);
        public Task<OAS?> GetOAS(int oasId);
        Task<int> GetOASIdByUsernameAsync(string username);
        Task<IEnumerable<OAS>?> GetAllOASAsync();
    }
}
