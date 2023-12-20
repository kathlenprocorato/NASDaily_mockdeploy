using AutoMapper;
using CITNASDaily.Entities.Dtos.ActivitiesSummaryDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Services
{
    public class ActivitiesSummaryService : IActivitiesSummaryService
    {
        public readonly IActivitiesSummaryRepository _activitiesSummaryRepository;
        private readonly IMapper _mapper;
        public ActivitiesSummaryService(IActivitiesSummaryRepository activitiesSummaryRepository, IMapper mapper)
        {
            _activitiesSummaryRepository = activitiesSummaryRepository;
            _mapper = mapper;
        }
        public async Task<ActivitiesSummary?> CreateActivitiesSummaryAsync(ActivitiesSummaryCreateDto activitiesSummaryDto, int nasId, int year, Semester semester)
        {
            var activitiesSummary = _mapper.Map<ActivitiesSummary>(activitiesSummaryDto);
            var createdActSummary = await _activitiesSummaryRepository.CreateActivitiesSummaryAsync(activitiesSummary, nasId, year, semester);
            return createdActSummary;
        }

        public async Task<IEnumerable<ActivitiesSummary>?> GetAllActivitiesSummaryAsync()
        {
            return await _activitiesSummaryRepository.GetAllActivitiesSummaryAsync();
        }

        public async Task<List<ActivitiesSummary>?> GetAllActivitiesSummaryByNASIdAsync(int nasId)
        {
            var actSummaries = await _activitiesSummaryRepository.GetAllActivitiesSummaryByNASIdAsync(nasId);
            if (actSummaries != null)
            {
                return actSummaries.ToList();
            }
            return null;
           
        }

        public async Task<List<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdMonthYearAsync(int nasId, int month, int year)
        {
            var actSummaries = await _activitiesSummaryRepository.GetAllActivitiesSummaryByNASIdMonthYearAsync(nasId, month, year);
            return actSummaries.ToList();
        }

        public async Task<List<ActivitiesSummary?>> GetAllActivitiesSummaryByNASIdYearSemesterAsync(int nasId, int year, Semester semester)
        {
            var actSummaries = await _activitiesSummaryRepository.GetAllActivitiesSummaryByNASIdYearSemesterAsync(nasId, year, semester);
            return actSummaries.ToList();
        }
    }
}
