using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface ITimekeepingSummaryRepository
    {
        Task<TimekeepingSummary?> CreateTimekeepingSummaryAsync(TimekeepingSummary timekeepingSummary, int nasId, int year, Semester semester);
        Task<IEnumerable<TimekeepingSummary>?> GetAllTimekeepingSummaryAsync();
        Task<IQueryable<TimekeepingSummary>?> GetAllTimekeepingSummaryByNASIdAsync(int nasId);
        Task<TimekeepingSummary?> GetTimekeepingSummaryByNASIdSemesterYearAsync(int nasId, Semester semester, int year);
        Task<TimekeepingSummary?> UpdateTimekeepingSummaryAsync(int nasId, int year, Semester semester, TimekeepingSummary tk);
    }
}
