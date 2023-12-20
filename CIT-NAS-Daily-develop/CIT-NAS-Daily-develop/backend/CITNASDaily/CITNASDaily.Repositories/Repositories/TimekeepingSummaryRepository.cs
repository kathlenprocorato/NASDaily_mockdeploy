using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Repositories
{
    public class TimekeepingSummaryRepository : ITimekeepingSummaryRepository
    {
        private readonly NASContext _context;
        public TimekeepingSummaryRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<TimekeepingSummary?> CreateTimekeepingSummaryAsync(TimekeepingSummary timekeepingSummary, int nasId, int year, Semester semester)
        {
            var existingNAS = await _context.NAS
                                .SingleOrDefaultAsync(e => e.Id == nasId);

            var existingSYSem = await _context.NASSchoolYears
                                .SingleOrDefaultAsync(e => e.NASId == nasId && e.Year == year && e.Semester == semester);

            if (existingNAS == null || existingSYSem == null)
            {
                return null;
            }

            //timekeeping status
            if (timekeepingSummary.Excused == 0 && timekeepingSummary.Unexcused == 0 && timekeepingSummary.FailedToPunch == 0
                && timekeepingSummary.LateOver10Mins == 0 && timekeepingSummary.LateOver45Mins == 0 && timekeepingSummary.MakeUpDutyHours == 0)
            {
                timekeepingSummary.TimekeepingStatus = "EXCELLENT";
            }
            else if ((timekeepingSummary.Excused <= 3 && timekeepingSummary.Excused >= 0) &&
                (timekeepingSummary.Unexcused <= 3 && timekeepingSummary.Unexcused >= 0) &&
                (timekeepingSummary.FailedToPunch <= 3 && timekeepingSummary.FailedToPunch >= 0) &&
                (timekeepingSummary.LateOver10Mins <= 3 && timekeepingSummary.LateOver10Mins >= 0) &&
                (timekeepingSummary.LateOver45Mins <= 3 && timekeepingSummary.LateOver45Mins >= 0) &&
                (timekeepingSummary.MakeUpDutyHours <= 5 && timekeepingSummary.MakeUpDutyHours >= 0))
            {
                timekeepingSummary.TimekeepingStatus = "GOOD";
            }
            else
            {
                timekeepingSummary.TimekeepingStatus = "POOR";
            }

            if (Enum.IsDefined(typeof(Semester), semester)) {
                //checks time keeping summary with the same semester already exists
                var existingSummary = await _context.TimekeepingSummaries.FirstOrDefaultAsync(s => s.NASId == nasId && s.Semester == semester && s.SchoolYear == year);
                if (existingSummary == null)
                {
                    SummaryEvaluation sumEval = new SummaryEvaluation
                    {
                        nasId = nasId,
                        Semester = semester,
                        SchoolYear = year,
                        TimekeepingStatus = timekeepingSummary.TimekeepingStatus,
                        UnitsAllowed = existingNAS.UnitsAllowed
                    };
                    timekeepingSummary.NASId = nasId;
                    timekeepingSummary.Semester = semester;
                    timekeepingSummary.SchoolYear = year;
                    await _context.SummaryEvaluations.AddAsync(sumEval);
                    await _context.TimekeepingSummaries.AddAsync(timekeepingSummary);
                    await _context.SaveChangesAsync();
                    return timekeepingSummary;
                }
                return null;
            }
            return null;
        }

        public async Task<IEnumerable<TimekeepingSummary>?> GetAllTimekeepingSummaryAsync()
        {
            return await _context.TimekeepingSummaries.ToListAsync();
        }

        public async Task<IQueryable<TimekeepingSummary>?> GetAllTimekeepingSummaryByNASIdAsync(int nasId)
        {
            return await Task.FromResult(_context.TimekeepingSummaries.Where(e => e.NASId == nasId));
        }

        public async Task<TimekeepingSummary?> GetTimekeepingSummaryByNASIdSemesterYearAsync(int nasId, Semester semester, int year)
        {
            return await _context.TimekeepingSummaries.FirstOrDefaultAsync(s => s.NASId == nasId && s.Semester == semester && s.SchoolYear == year);
        }

        public async Task<TimekeepingSummary?> UpdateTimekeepingSummaryAsync(int nasId, int year, Semester semester, TimekeepingSummary tk)
        {
            var existingTK = await _context.TimekeepingSummaries.FirstOrDefaultAsync(t => t.NASId == nasId && t.SchoolYear == year && t.Semester == semester);
            
            if (existingTK != null)
            {
                existingTK.Excused = tk.Excused;
                existingTK.Unexcused = tk.Unexcused;
                existingTK.MakeUpDutyHours = tk.MakeUpDutyHours;

                //timekeeping status
                if (existingTK.Excused == 0 && existingTK.Unexcused == 0 && existingTK.MakeUpDutyHours == 0)
                {
                    existingTK.TimekeepingStatus = "EXCELLENT";
                }
                else if ((existingTK.Excused <= 3 && existingTK.Excused >= 0) &&
                    (existingTK.Unexcused <= 3 && existingTK.Unexcused >= 0) &&
                    (existingTK.MakeUpDutyHours <= 5 && existingTK.MakeUpDutyHours >= 0))
                {
                    existingTK.TimekeepingStatus = "GOOD";
                }
                else
                {
                    existingTK.TimekeepingStatus = "POOR";
                }

                var existingSumEval = await _context.SummaryEvaluations.FirstOrDefaultAsync(s => s.nasId == nasId && s.SchoolYear == year && s.Semester == semester);
                if (existingSumEval != null)
                {
                    existingSumEval.TimekeepingStatus = existingTK.TimekeepingStatus;
                }

                await _context.SaveChangesAsync();

                return existingTK;
            }

            return null;
        }
    }
}
