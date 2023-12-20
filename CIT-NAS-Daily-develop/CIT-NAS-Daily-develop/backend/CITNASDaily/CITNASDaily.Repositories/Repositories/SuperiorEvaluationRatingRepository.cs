using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Repositories
{
    public class SuperiorEvaluationRatingRepository : ISuperiorEvaluationRatingRepository
    {
        private readonly NASContext _context;

        public SuperiorEvaluationRatingRepository(NASContext context)
        {
            _context = context;
        }

        public async Task<SuperiorEvaluationRating?> CreateSuperiorEvaluationRatingAsync(SuperiorEvaluationRating evaluation)
        {
            var existingNAS = await _context.NAS
                                .SingleOrDefaultAsync(e => e.Id == evaluation.NASId);

            var existingSYSem = await _context.NASSchoolYears
                                .SingleOrDefaultAsync(e => e.NASId == evaluation.NASId && e.Year == evaluation.SchoolYear && e.Semester == evaluation.Semester);

            var existingTKSummary = await _context.TimekeepingSummaries
                                .Where(r => r.NASId == evaluation.NASId && r.Semester == evaluation.Semester && r.SchoolYear == evaluation.SchoolYear)
                                .FirstOrDefaultAsync();

            var existingEvaluation = await _context.SuperiorEvaluationRatings
                                .Where(r => r.NASId == evaluation.NASId && r.Semester == evaluation.Semester && r.SchoolYear == evaluation.SchoolYear)
                                .FirstOrDefaultAsync();

            if (existingNAS == null || existingSYSem == null || existingTKSummary == null || existingEvaluation != null)
            {
                return null;
            }

            if (evaluation.AttendanceAndPunctuality >= 0 && evaluation.AttendanceAndPunctuality <= 10
                && evaluation.QualOfWorkOutput >= 0 && evaluation.QualOfWorkOutput <= 15
                && evaluation.QuanOfWorkOutput >= 0 && evaluation.QuanOfWorkOutput <= 10
                && evaluation.AttitudeAndWorkBehaviour >= 0 && evaluation.AttitudeAndWorkBehaviour <= 25
                && evaluation.OverallAssessment >= 0 && evaluation.OverallAssessment <= 5)
            {
                //calculation for overallrating
                var overAllRating = (float)(evaluation.AttendanceAndPunctuality + evaluation.QualOfWorkOutput + evaluation.QuanOfWorkOutput
                    + evaluation.AttitudeAndWorkBehaviour + evaluation.OverallAssessment)/13;
                evaluation.OverallRating = (float)Math.Round(overAllRating, 2);
                evaluation.Semester = existingTKSummary.Semester;
                evaluation.SchoolYear = existingTKSummary.SchoolYear;
                await _context.SuperiorEvaluationRatings.AddAsync(evaluation);
                await _context.SaveChangesAsync();
                return evaluation;
            }

            return null;
        }

        public async Task<SuperiorEvaluationRating?> GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(int nasId, Semester semester, int year)
        {
            return await _context.SuperiorEvaluationRatings.FirstOrDefaultAsync(s => s.NASId == nasId && s.Semester == semester && s.SchoolYear == year);
        }
    }
}
