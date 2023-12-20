using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface ISummaryEvaluationService
    {
        Task<SummaryEvaluation?> CreateSummaryEvaluationAsync(SummaryEvaluationCreateDto summaryEvaluation);
        Task<IEnumerable<SummaryEvaluationDto?>> GetSummaryEvaluationsAsync();
        Task<SummaryEvaluation?> GetSummaryEvaluationByNASIdSemesterYearAsync(int nasId, Semester semester, int year);
        Task<SummaryEvaluation?> UpdateSummaryEvaluationAsync(SummaryEvaluationUpdateDto summaryEvaluation);
        Task<SummaryEvaluation?> UploadGrades(int nasId, int year, Semester semester, IFormFile file);
        Task<byte[]?> GetNASGradePicture(int nasId, int year, Semester semester);
        Task<SummaryEvaluation?> UpdateSuperiorRatingAsync(int nasId, int year, Semester semester, float rating);
    }
}
