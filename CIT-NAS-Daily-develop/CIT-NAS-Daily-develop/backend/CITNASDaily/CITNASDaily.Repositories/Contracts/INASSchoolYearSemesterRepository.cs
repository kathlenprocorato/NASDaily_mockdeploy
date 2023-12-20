using CITNASDaily.Entities.Dtos.SchoolYearDto;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface INASSchoolYearSemesterRepository
    {
        Task<List<NASSchoolYearSemester>?> AddSchoolYearSemesterAsync(int nasId, List<NASSchoolYearSemesterCreateDto> data);
        Task<IEnumerable<NASSchoolYearSemester?>> GetSchoolYearSemesterAsync(int nasId);
        Task<List<int>> GetAllNasIdBySYSemesterAsync(int year, Semester semester);
        Task<List<NASSYSemOnly>> GetAllSYAndSem();
    }
}
