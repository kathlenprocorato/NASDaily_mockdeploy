using CITNASDaily.Entities.Models;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IBiometricLogRepository
    {
        Task<BiometricLog?> CreateLogAsync(BiometricLog log);
        Task<IEnumerable<BiometricLog?>> GetNASLogsAsync(int nasId);
    }
}
