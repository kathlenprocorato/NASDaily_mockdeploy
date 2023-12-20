using AutoMapper;
using CITNASDaily.Entities.Dtos.DailyTimeRecordDto;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Services
{
    public class DTRService : IDTRService
    {
        private readonly IDTRRepository _dtrRepository;
        private readonly IMapper _mapper;

        public DTRService(IDTRRepository dtrRepository, IMapper mapper)
        {
            _dtrRepository = dtrRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DailyTimeRecord>?> GetAllDTRAsync()
        {
            return await _dtrRepository.GetDTRs();
        }

        public async Task<IEnumerable<DailyTimeRecord>?> GetDTRByNasNameAsync(string firstName, string lastName, string middleName)
        {
            return await _dtrRepository.GetDTRByNasNameAsync(firstName, lastName, middleName);
        }

        public async Task SaveDTRs(IFormFile file, int year, Semester semester)
        {
            await _dtrRepository.SaveDTRs(file, year, semester);
        }

        public async Task<DailyTimeRecordListDto> GetDTRsBySYSemesterAsync(int year, Semester semester, string firstName, string lastName, string middleName)
        {
            var dtrList = await _dtrRepository.GetDTRsBySYSemesterAsync(year, semester, firstName, lastName, middleName);

            var dtrDto = _mapper.Map<List<DailyTimeRecordDto>>(dtrList);

            DailyTimeRecordListDto dailyTimeRecordListDto = new DailyTimeRecordListDto
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                SchoolYear = year,
                Semester = semester,
                DailyTimeRecords = dtrDto
            };

            return dailyTimeRecordListDto;
        }
    }
}
