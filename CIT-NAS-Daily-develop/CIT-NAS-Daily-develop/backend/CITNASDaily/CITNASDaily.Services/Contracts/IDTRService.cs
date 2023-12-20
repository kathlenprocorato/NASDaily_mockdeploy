using CITNASDaily.Entities.Dtos.DailyTimeRecordDto;
using CITNASDaily.Entities.Models;
using Microsoft.AspNetCore.Http;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface IDTRService
    {
        Task<IEnumerable<DailyTimeRecord>?> GetAllDTRAsync();
        Task SaveDTRs(IFormFile file, int year, Semester semester);
        Task<IEnumerable<DailyTimeRecord>?> GetDTRByNasNameAsync(string firstName, string lastName, string middleName);
        Task<DailyTimeRecordListDto> GetDTRsBySYSemesterAsync(int year, Semester semester, string firstName, string lastName, string middleName);
    }
}
