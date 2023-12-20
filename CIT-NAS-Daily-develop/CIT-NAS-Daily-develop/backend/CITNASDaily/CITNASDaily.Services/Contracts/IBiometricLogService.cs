using CITNASDaily.Entities.Dtos.BiometricLogDtos;
using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.UserDtos;

namespace CITNASDaily.Services.Contracts
{
    public interface IBiometricLogService
    {
        Task<BiometricLogDto?> CreateLogAsync(int nasId, BiometricLogCreateDto logCreate);
        Task<IEnumerable<BiometricLogDto?>> GetNASLogsAsync(int nasId);
    }
}
