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
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly NASContext _context;

        public ScheduleRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<Schedule?> CreateScheduleAsync(Schedule schedule)
        {
            var existingNAS = await _context.NAS
                                .SingleOrDefaultAsync(e => e.Id == schedule.NASId);

            var existingSYSem = await _context.NASSchoolYears
                                .SingleOrDefaultAsync(e => e.NASId == schedule.NASId && e.Year == schedule.SchoolYear && e.Semester == schedule.Semester);

            if (existingNAS == null || existingSYSem == null)
            {
                return null;
            }

            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<IQueryable<Schedule?>> GetSchedulesByNASIdAsync(int nasId)
        {
            return await Task.FromResult(_context.Schedules.Where(s => s.NASId == nasId));
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByNASIdSYSemesterAsync(int nasId, int year, Semester semester)
        {
            return await _context.Schedules.Where(s => s.NASId == nasId && s.Semester == semester && s.SchoolYear == year).ToListAsync();
        }

        public async Task DeleteSchedulesByNASIdAsync(int nasId)
        {
            var existingSchedules = await _context.Schedules
                                    .Where(schedule => schedule.NASId == nasId)
                                    .ToListAsync();
            if (existingSchedules.Any())
            {
                _context.Schedules.RemoveRange(existingSchedules);
                await _context.SaveChangesAsync();
            }
        }
    }
}
