using AutoMapper;
using CITNASDaily.Entities.Dtos.DailyTimeRecordDto;
using CITNASDaily.Entities.Dtos.ScheduleDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Services
{
    public class ScheduleService : IScheduleService
    {
        public readonly IScheduleRepository _scheduleRepository;
        public readonly IMapper _mapper;

        public ScheduleService(IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        public async Task<Schedule> CreateScheduleAsync(ScheduleCreateDto schedule)
        {
            var sched = _mapper.Map<Schedule>(schedule);
            var createdSched = await _scheduleRepository.CreateScheduleAsync(sched);

            if(createdSched != null)
            {
                return createdSched;
            }

            return null;
        }

        public async Task<List<Schedule?>> GetSchedulesByNASIdAsync(int nasId)
        {
            var sched = await _scheduleRepository.GetSchedulesByNASIdAsync(nasId);
            if(!sched.IsNullOrEmpty())
            {
                return sched.ToList();
            }
            return null;
        }

        public async Task<ScheduleListDto> GetSchedulesByNASIdSYSemesterAsync(int nasId, int year, Semester semester)
        {
            var schedList = await _scheduleRepository.GetSchedulesByNASIdSYSemesterAsync(nasId, year, semester);

            var schedDto = _mapper.Map<List<ScheduleDto>>(schedList);

            ScheduleListDto scheduleListDto = new ScheduleListDto
            {
                NASId = nasId,
                SchoolYear = year,
                Semester = semester,
                Schedules = schedDto
            };

            return scheduleListDto;
        }

        public async Task DeleteSchedulesByNASIdAsync(int nasId)
        {
            await _scheduleRepository.DeleteSchedulesByNASIdAsync(nasId);
        }
    }
}
