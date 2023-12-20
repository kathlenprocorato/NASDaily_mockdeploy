using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Repositories
{
    public class SummaryEvaluationRepository : ISummaryEvaluationRepository
    {
        private readonly NASContext _context;

        public SummaryEvaluationRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<SummaryEvaluation?> CreateSummaryEvaluationAsync(SummaryEvaluation summaryEvaluation)
        {
            var existingNAS = await _context.NAS
                                .SingleOrDefaultAsync(e => e.Id == summaryEvaluation.nasId);

            var existingSYSem = await _context.NASSchoolYears
                                .SingleOrDefaultAsync(e => e.NASId == summaryEvaluation.nasId && e.Year == summaryEvaluation.SchoolYear && e.Semester == summaryEvaluation.Semester);

            var existingSummary = await _context.SummaryEvaluations
                                .SingleOrDefaultAsync(s => s.nasId == summaryEvaluation.nasId && s.Semester == summaryEvaluation.Semester && s.SchoolYear == summaryEvaluation.SchoolYear);

            if (existingNAS == null || existingSYSem == null || existingSummary != null)
            {
                return null;
            }

            if (Enum.IsDefined(typeof(Semester), summaryEvaluation.Semester))
            {
                await _context.SummaryEvaluations.AddAsync(summaryEvaluation);
                await _context.SaveChangesAsync();
                return summaryEvaluation;
            }

            return null;
        }

        public async Task<IEnumerable<SummaryEvaluation?>> GetSummaryEvaluationsAsync()
        {
            return await _context.SummaryEvaluations.ToListAsync();
        }

        public async Task<SummaryEvaluation?> GetSummaryEvaluationByNASIdSemesterYearAsync(int nasId, Semester semester, int year)
        {
            return await _context.SummaryEvaluations.FirstOrDefaultAsync(s => s.nasId == nasId && s.Semester == semester && s.SchoolYear == year);
        }

        public async Task<SummaryEvaluation?> UpdateSummaryEvaluationAsync(SummaryEvaluation summaryEvaluation)
        {
            var existingEval = await _context.SummaryEvaluations
                                                .Where(se => se.nasId == summaryEvaluation.nasId && se.Semester == summaryEvaluation.Semester && se.SchoolYear == summaryEvaluation.SchoolYear)
                                                .FirstOrDefaultAsync();
            var nas = await _context.NAS
                .FindAsync(summaryEvaluation.nasId);

            if (existingEval != null && nas != null)
            {
                existingEval.AllCoursesPassed = summaryEvaluation.AllCoursesPassed;
                existingEval.NoOfCoursesFailed = summaryEvaluation.NoOfCoursesFailed;
                existingEval.Responded = summaryEvaluation.Responded;
                existingEval.EnrollmentAllowed = summaryEvaluation.EnrollmentAllowed;

                if(existingEval.NoOfCoursesFailed > 0)
                {
                    existingEval.UnitsAllowed = nas.UnitsAllowed - (3 * existingEval.NoOfCoursesFailed);
                    nas.UnitsAllowed = existingEval.UnitsAllowed;
                }

                await _context.SaveChangesAsync();

                return existingEval;
            }

            return null;
        }

        public async Task<SummaryEvaluation?> UploadGrades(int nasId, int year, Semester semester, IFormFile file)
        {
            var existingEval = await _context.SummaryEvaluations
                                                .Where(se => se.nasId == nasId && se.Semester == semester && se.SchoolYear == year)
                                                .FirstOrDefaultAsync();

            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (existingEval != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();
                    existingEval.AcademicPerformance = fileData;
                    await _context.SaveChangesAsync();
                    return existingEval;
                }
            }

            return null;
        }

        public async Task<byte[]?> GetNASGradePicture(int nasId, int year, Semester semester)
        {
            var existingEval = await _context.SummaryEvaluations
                                                .Where(se => se.nasId == nasId && se.Semester == semester && se.SchoolYear == year)
                                                .FirstOrDefaultAsync();

            return existingEval?.AcademicPerformance;
        }

        public async Task<SummaryEvaluation?> UpdateSuperiorRatingAsync(int nasId, int year, Semester semester, float rating)
        {
            var existingEval = await _context.SummaryEvaluations
                                                .Where(se => se.nasId == nasId && se.Semester == semester && se.SchoolYear == year)
                                                .FirstOrDefaultAsync();

            if(existingEval != null)
            {
                existingEval.SuperiorOverallRating = rating;
                await _context.SaveChangesAsync();
                return existingEval;
            }

            return null;
        }
    }
}
