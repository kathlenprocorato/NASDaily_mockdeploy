using CITNASDaily.Entities.Dtos.ScheduleDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface IScheduleService
    {
        Task<Schedule> CreateScheduleAsync(ScheduleCreateDto schedule);
        Task<List<Schedule?>> GetSchedulesByNASIdAsync(int nasId);
        Task<ScheduleListDto> GetSchedulesByNASIdSYSemesterAsync(int nasId, int year, Semester semester);
        Task DeleteSchedulesByNASIdAsync(int nasId);
    }
}
