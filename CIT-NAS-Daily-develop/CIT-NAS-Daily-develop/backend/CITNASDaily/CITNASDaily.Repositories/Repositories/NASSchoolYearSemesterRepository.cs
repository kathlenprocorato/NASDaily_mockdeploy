using CITNASDaily.Entities.Dtos.SchoolYearDto;
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
    public class NASSchoolYearSemesterRepository : INASSchoolYearSemesterRepository
    {
        private readonly NASContext _context;

        public NASSchoolYearSemesterRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<List<NASSchoolYearSemester>?> AddSchoolYearSemesterAsync(int nasId, List<NASSchoolYearSemesterCreateDto> data)
        {
            var addedSchoolYears = new List<NASSchoolYearSemester>();

            foreach (var sy in data)
            {
                var schoolyear = new NASSchoolYearSemester
                {
                    NASId = nasId,
                    Year = sy.Year,
                    Semester = sy.Semester
                };

                await _context.NASSchoolYears.AddAsync(schoolyear);
                addedSchoolYears.Add(schoolyear);
            }

            await _context.SaveChangesAsync();

            return addedSchoolYears;
        }

        public async Task<IEnumerable<NASSchoolYearSemester?>> GetSchoolYearSemesterAsync(int nasId)
        {
            return await _context.NASSchoolYears.Where(n => n.NASId == nasId).ToListAsync();
        }

        public async Task<List<int>> GetAllNasIdBySYSemesterAsync(int year, Semester semester)
        {
            var sySemList = await _context.NASSchoolYears.Where(s => s.Year == year && s.Semester == semester).ToListAsync();

            var nasIdList = new List<int>();

            foreach (var nas in sySemList)
            {
                nasIdList.Add(nas.NASId);
            }

            return nasIdList;
        }

        public async Task<List<NASSYSemOnly>> GetAllSYAndSem()
        {
            return await _context.NASSchoolYears
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Semester)
                .Select(s => new NASSYSemOnly { Year = s.Year, Semester = s.Semester })
                .Distinct()
                .ToListAsync();
        }
    }
}
