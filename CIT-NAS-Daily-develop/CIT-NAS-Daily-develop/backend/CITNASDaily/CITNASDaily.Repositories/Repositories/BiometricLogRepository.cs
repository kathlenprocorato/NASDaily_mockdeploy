using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CITNASDaily.Repositories.Repositories
{
    public class BiometricLogRepository : IBiometricLogRepository
    {
        private readonly NASContext _context;

        public BiometricLogRepository(NASContext context)
        {
            _context = context;
        }
        public async Task<BiometricLog?> CreateLogAsync(BiometricLog log)
        {
            await _context.BiometricLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<IEnumerable<BiometricLog?>> GetNASLogsAsync(int nasId)
        {
            return await _context.BiometricLogs.Where(b => b.NASId == nasId).ToListAsync();
        }
    }
}
