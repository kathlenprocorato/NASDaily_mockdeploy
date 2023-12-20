using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IActivitiesSummaryRepository
    {
        Task<ActivitiesSummary?> CreateActivitiesSummaryAsync(ActivitiesSummary activitiesSummary, int nasId, int year, Semester semester);
        Task<IEnumerable<ActivitiesSummary>?> GetAllActivitiesSummaryAsync();
        Task<IQueryable<ActivitiesSummary>?> GetAllActivitiesSummaryByNASIdAsync(int nasId);
        Task<IQueryable<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdMonthYearAsync(int nasId, int month, int year);
        Task<IQueryable<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdYearSemesterAsync(int nasId, int year, Semester semester);
    }
}
