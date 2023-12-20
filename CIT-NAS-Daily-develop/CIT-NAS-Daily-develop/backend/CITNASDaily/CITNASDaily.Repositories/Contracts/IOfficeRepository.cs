using CITNASDaily.Entities.Models;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IOfficeRepository
    {
        Task<Office?> CreateOfficeAsync(Office office);
        Task<IEnumerable<Office?>> GetOfficesAsync();
        Task<Office?> GetOfficeByIdAsync(int id);
        Task<Office?> GetOfficeBySuperiorIdAsync(int superiorId);
        Task<Office?> GetOfficeByNASIdAsync(int nasId);
        Task<string?> GetOfficeNameAsync(int officeId);
        Task<Office?> UpdateOfficeAsync(Office office);
    }
}
