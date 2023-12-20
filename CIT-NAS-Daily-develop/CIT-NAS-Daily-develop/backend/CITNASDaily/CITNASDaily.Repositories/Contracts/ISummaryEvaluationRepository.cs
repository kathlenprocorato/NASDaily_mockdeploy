using CITNASDaily.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface ISummaryEvaluationRepository
    {
        Task<SummaryEvaluation?> CreateSummaryEvaluationAsync(SummaryEvaluation summaryEvaluation);
        Task<IEnumerable<SummaryEvaluation?>> GetSummaryEvaluationsAsync();
        Task<SummaryEvaluation?> GetSummaryEvaluationByNASIdSemesterYearAsync(int nasId, Semester semester, int year);
        Task<SummaryEvaluation?> UpdateSummaryEvaluationAsync(SummaryEvaluation summaryEvaluation);
        Task<SummaryEvaluation?> UploadGrades(int nasId, int year, Semester semester, IFormFile file);
        Task<byte[]?> GetNASGradePicture(int nasId, int year, Semester semester);
        Task<SummaryEvaluation?> UpdateSuperiorRatingAsync(int nasId, int year, Semester semester, float rating);
    }
}
