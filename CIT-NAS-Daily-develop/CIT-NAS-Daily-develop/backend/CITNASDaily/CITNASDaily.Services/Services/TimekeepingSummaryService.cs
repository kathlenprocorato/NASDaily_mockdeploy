using AutoMapper;
using CITNASDaily.Entities.Dtos.TimekeepingSummaryDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Services
{
    public class TimekeepingSummaryService : ITimekeepingSummaryService
    {
        public readonly ITimekeepingSummaryRepository _timekeepingSummaryRepository;
        private readonly IMapper _mapper;
        public TimekeepingSummaryService(ITimekeepingSummaryRepository timekeepingSummaryRepository, IMapper mapper)
        {
            _timekeepingSummaryRepository = timekeepingSummaryRepository;
            _mapper = mapper;
        }

        public async Task<TimekeepingSummary?> CreateTimekeepingSummaryAsync(TimekeepingSummaryCreateDto timekeepingSummaryDto, int nasId, int year, Semester semester)
        {
            var timekeepingSummary = _mapper.Map<TimekeepingSummary>(timekeepingSummaryDto);
           
            var createdActSummary = await _timekeepingSummaryRepository.CreateTimekeepingSummaryAsync(timekeepingSummary, nasId, year, semester);

            if (createdActSummary != null)
            {
                return createdActSummary;
            }
            return null;
        }

        public async Task<IEnumerable<TimekeepingSummary>?> GetAllTimekeepingSummaryAsync()
        {
            return await _timekeepingSummaryRepository.GetAllTimekeepingSummaryAsync();
        }

        public async Task<List<TimekeepingSummary>?> GetAllTimekeepingSummaryByNASIdAsync(int nasId)
        {
            var timekeepingSummary = await _timekeepingSummaryRepository.GetAllTimekeepingSummaryByNASIdAsync(nasId);
            if (timekeepingSummary != null)
            {
                return timekeepingSummary.ToList();
            }
            return null;
        }

        public async Task<TimekeepingSummary?> GetTimekeepingSummaryByNASIdSemesterYearAsync(int nasId, Semester semester, int year)
        {
            return await _timekeepingSummaryRepository.GetTimekeepingSummaryByNASIdSemesterYearAsync(nasId, semester, year);
        }

        public async Task<TimekeepingSummary?> UpdateTimekeepingSummaryAsync(int nasId, int year, Semester semester, TimekeepingSummaryUpdateDto tkUpdate)
        {
            var tk = _mapper.Map<TimekeepingSummary>(tkUpdate);

            return await _timekeepingSummaryRepository.UpdateTimekeepingSummaryAsync(nasId, year, semester, tk);
        }
    }
}
