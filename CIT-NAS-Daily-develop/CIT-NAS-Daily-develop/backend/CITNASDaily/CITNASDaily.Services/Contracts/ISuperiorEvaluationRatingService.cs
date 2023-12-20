using CITNASDaily.Entities.Dtos.SuperiorEvaluationRatingDto;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface ISuperiorEvaluationRatingService
    {
        Task<SuperiorEvaluationRating?> CreateSuperiorEvaluationRatingAsync(SuperiorEvaluationRatingCreateDto SuperiorEvaluationRatingDto);
        Task<SuperiorEvaluationRating?> GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(int nasId, Semester semester, int year);
    }
}
