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
    public class ActivitiesSummaryRepository : IActivitiesSummaryRepository
    {
        private readonly NASContext _context;

        public ActivitiesSummaryRepository(NASContext context)
        {
            _context = context;
        }
        public async Task<ActivitiesSummary?> CreateActivitiesSummaryAsync(ActivitiesSummary activitiesSummary, int nasId, int year, Semester semester)
        {
            var existingNAS = await _context.NAS
                                .SingleOrDefaultAsync(e => e.Id == nasId);

            var existingSYSem = await _context.NASSchoolYears
                                .SingleOrDefaultAsync(e => e.NASId == nasId && e.Year == year && e.Semester == semester);

            if (existingNAS == null || existingSYSem == null)
            {
                return null;
            }

            activitiesSummary.NASId = nasId;
            activitiesSummary.SchoolYear = year;
            activitiesSummary.Semester = semester;
            activitiesSummary.DateOfEntry = DateTime.Now;

            await _context.ActivitiesSummaries.AddAsync(activitiesSummary);
            await _context.SaveChangesAsync();

            return activitiesSummary;
        }

        public async Task<IEnumerable<ActivitiesSummary>?> GetAllActivitiesSummaryAsync()
        {
            return await _context.ActivitiesSummaries.ToListAsync();
        }

        public async Task<IQueryable<ActivitiesSummary>?> GetAllActivitiesSummaryByNASIdAsync(int nasId)
        {
            return await Task.FromResult(_context.ActivitiesSummaries.Where(e => e.NASId == nasId));
        }

        public async Task<IQueryable<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdMonthYearAsync(int nasId, int month, int year)
        {
            return await Task.FromResult(_context.ActivitiesSummaries.Where(e => e.NASId == nasId && e.DateOfEntry.Month == month && e.DateOfEntry.Year == year));
        }

        public async Task<IQueryable<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdYearSemesterAsync(int nasId, int year, Semester semester)
        {
            return await Task.FromResult(_context.ActivitiesSummaries.Where(e => e.NASId == nasId && e.SchoolYear == year && e.Semester == semester));
        }
    }
}
