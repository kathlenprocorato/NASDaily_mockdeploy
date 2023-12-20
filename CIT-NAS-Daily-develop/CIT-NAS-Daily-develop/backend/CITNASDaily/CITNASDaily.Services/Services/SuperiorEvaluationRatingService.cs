using AutoMapper;
using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Dtos.SuperiorEvaluationRatingDto;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Services
{
    public class SuperiorEvaluationRatingService : ISuperiorEvaluationRatingService
    {
        public readonly ISuperiorEvaluationRatingRepository _superiorEvaluationRatingRepository;
        private readonly IMapper _mapper;
        public SuperiorEvaluationRatingService(ISuperiorEvaluationRatingRepository superiorEvaluationRatingRepository, IMapper mapper)
        {
            _superiorEvaluationRatingRepository = superiorEvaluationRatingRepository;
            _mapper = mapper;
        }

        public async Task<SuperiorEvaluationRating?> CreateSuperiorEvaluationRatingAsync(SuperiorEvaluationRatingCreateDto SuperiorEvaluationRatingDto)
        {
            var superiorEvaluationRating = _mapper.Map<SuperiorEvaluationRating>(SuperiorEvaluationRatingDto);
            return await _superiorEvaluationRatingRepository.CreateSuperiorEvaluationRatingAsync(superiorEvaluationRating);
        }

        public async Task<SuperiorEvaluationRating?> GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(int nasId, Semester semester, int year)
        {
            var evaluationRating = await _superiorEvaluationRatingRepository.GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(nasId, semester, year);
            return await _superiorEvaluationRatingRepository.GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(nasId, semester, year);
        }
    }
}
