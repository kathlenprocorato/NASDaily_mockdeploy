using CITNASDaily.Entities.Dtos.OfficeDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Services.Contracts
{
    public interface IOfficeService
    {
        Task<Office?> CreateOfficeAsync(OfficeCreateDto office);
        Task<IEnumerable<Office?>> GetOfficesAsync();
        Task<Office?> GetOfficeByIdAsync(int id);
        Task<Office?> GetOfficeBySuperiorIdAsync(int superiorId);
        Task<Office?> GetOfficeByNASIdAsync(int nasId);
        Task<OfficeDto?> UpdateOfficeAsync(OfficeUpdateDto office);
    }
}
