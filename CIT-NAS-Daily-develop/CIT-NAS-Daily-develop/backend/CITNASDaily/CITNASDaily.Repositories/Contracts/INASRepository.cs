using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.SchoolYearDto;
using CITNASDaily.Entities.Models;
using Microsoft.AspNetCore.Http;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Repositories.Contracts
{
    public interface INASRepository
    {
        Task<NAS?> CreateNASAsync(NAS nas);
        Task<NAS?> GetNASAsync(int nasId);
        Task<IEnumerable<NAS>?> GetAllNASAsync();
        Task<IEnumerable<NAS>?> GetAllNasBySYSemesterAsync(List<int> nasIdList);
        Task<int> GetNASIdByUsernameAsync(string username);
        Task<IEnumerable<NAS?>> GetNASByOfficeIdSYSemesterAsync(int officeId, int year, Semester semester);
        Task<NAS?> GetNASByNASIdSYSemesterAsync(int nasId, int year, Semester semester);
        Task<IEnumerable<NASSYSemOnly?>> GetSYSemByNASIdAsync(int nasId);
        Task<byte[]?> UploadPhotoAsync(int nasId, IFormFile file);
        Task<NAS?> UpdateNASAsync(int nasId, NAS nas);
        Task<bool> ChangePasswordAsync(int nasId, string newPassword);
    }
}
