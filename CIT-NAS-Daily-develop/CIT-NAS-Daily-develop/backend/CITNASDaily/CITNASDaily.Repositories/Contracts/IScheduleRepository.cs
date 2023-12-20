using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IScheduleRepository
    {
        Task<Schedule?> CreateScheduleAsync(Schedule schedule);
        Task<IQueryable<Schedule?>> GetSchedulesByNASIdAsync(int nasId);
        Task<IEnumerable<Schedule>> GetSchedulesByNASIdSYSemesterAsync(int nasId, int year, Semester semester);
        Task DeleteSchedulesByNASIdAsync(int nasId);
    }
}
