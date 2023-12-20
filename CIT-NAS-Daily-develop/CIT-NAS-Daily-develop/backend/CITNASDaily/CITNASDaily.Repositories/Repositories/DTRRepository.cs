using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Repositories
{
    public class DTRRepository : IDTRRepository
    {
        private readonly NASContext _context;

        public DTRRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyTimeRecord>> GetDTRByNasNameAsync(string firstName, string lastName, string middleName)
        {
            return await _context.DailyTimeRecords.Where(d => d.FirstName == firstName &&
                d.LastName == lastName && d.MiddleName == middleName).ToListAsync();
        }

        public async Task<IEnumerable<DailyTimeRecord>> GetDTRs()
        {
            return await _context.DailyTimeRecords.ToListAsync();
        }

        public async Task<IEnumerable<DailyTimeRecord>> GetDTRsBySYSemesterAsync(int year, Semester semester, string firstName, string lastName, string? middleName)
        {
            if(middleName == "")
            {
                middleName = null;
            }

            return await _context.DailyTimeRecords
                .Where(d => d.SchoolYear == year && d.Semester == semester && d.FirstName == firstName && 
                d.LastName == lastName && d.MiddleName == middleName)
                .ToListAsync();
        }

        public async Task SaveDTRs(IFormFile file, int year, Semester semester)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<DailyTimeRecord> records = new List<DailyTimeRecord>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        DailyTimeRecord record = new DailyTimeRecord
                        {
                            FirstName = worksheet.Cells[i, 1].Value?.ToString(),
                            MiddleName = worksheet.Cells[i, 2].Value?.ToString(),
                            LastName = worksheet.Cells[i, 3].Value?.ToString(),
                            Date = worksheet.Cells[i, 4].Value?.ToString(),
                            TimeIn = worksheet.Cells[i, 5].Value?.ToString(),
                            TimeOut = worksheet.Cells[i, 6].Value?.ToString(),
                            OvertimeIn = worksheet.Cells[i, 7].Value?.ToString(),
                            OvertimeOut = worksheet.Cells[i, 8].Value?.ToString(),
                            WorkTime = worksheet.Cells[i, 9].Value?.ToString(),
                            TotalWorkTime = worksheet.Cells[i, 10].Value?.ToString(),
                            SchoolYear = year,
                            Semester = (Semester)semester
                        };

                        records.Add(record);
                    }
                }
            }

            var existingDtr = await _context.DailyTimeRecords
                                    .Where(dtr => dtr.Semester == semester && dtr.SchoolYear == year)
                                    .ToListAsync();

            if (existingDtr.Any())
            {
                //there is data existing in the database, only need to do is to update, so we delete data to update
                _context.DailyTimeRecords.RemoveRange(existingDtr);
            }

            foreach (var dtr in records)
            {
                if (dtr.FirstName == null && dtr.MiddleName == null && dtr.LastName == null &&
                    dtr.Date == null && dtr.TimeIn == null && dtr.TimeOut == null &&
                    dtr.OvertimeIn == null && dtr.OvertimeOut == null && dtr.WorkTime == null
                    && dtr.TotalWorkTime == null)
                {
                    continue;
                }
                await _context.DailyTimeRecords.AddAsync(dtr);
            }

            await _context.SaveChangesAsync();
        }
    }
}
