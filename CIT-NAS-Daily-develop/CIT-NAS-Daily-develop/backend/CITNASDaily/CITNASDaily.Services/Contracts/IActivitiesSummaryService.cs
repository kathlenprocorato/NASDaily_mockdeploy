using CITNASDaily.Entities.Dtos.ActivitiesSummaryDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface IActivitiesSummaryService
    {
        Task<ActivitiesSummary?> CreateActivitiesSummaryAsync(ActivitiesSummaryCreateDto activitiesSummaryDto, int nasId, int year, Semester semester);
        Task<IEnumerable<ActivitiesSummary>?> GetAllActivitiesSummaryAsync();
        Task<List<ActivitiesSummary>?> GetAllActivitiesSummaryByNASIdAsync(int nasId);
        Task<List<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdMonthYearAsync(int nasId, int month, int year);
        Task<List<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdYearSemesterAsync(int nasId, int year, Semester semester);
    }
}
