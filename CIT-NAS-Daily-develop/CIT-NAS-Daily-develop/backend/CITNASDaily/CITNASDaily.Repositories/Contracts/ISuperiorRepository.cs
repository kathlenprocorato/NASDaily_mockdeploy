using CITNASDaily.Entities.Models;

namespace CITNASDaily.Repositories.Contracts
{
    public interface ISuperiorRepository
    {
        Task<Superior?> CreateSuperiorAsync(Superior superior);
        Task<Superior?> GetSuperiorAsync(int superiorId);
        Task<IEnumerable<Superior?>> GetSuperiorsAsync();
        public Task<Superior?> GetSuperiorByUserIdAsync(Guid? userId);
        public Task<Superior?> GetSuperiorByUsernameAsync(string username);
        Task<Superior?> GetSuperiorByOfficeId(int officeId);
        Task<int> GetSuperiorIdByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(int superiorId, string newPassword);
    }
}
