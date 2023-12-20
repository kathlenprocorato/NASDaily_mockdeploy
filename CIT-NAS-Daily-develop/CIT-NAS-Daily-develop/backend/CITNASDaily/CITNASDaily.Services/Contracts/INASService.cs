using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;
using Microsoft.AspNetCore.Http;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Services.Contracts
{
    public interface INASService
    {
        Task<NASDto?> CreateNASAsync(string username, NASCreateDto nasCreate);
        Task<NASDto?> GetNASAsync(int nasId);
        Task<NASDtoNoImage?> GetNASNoImageAsync(int nasId);
        Task<IEnumerable<NASDto>?> GetAllNASAsync();
        Task<IEnumerable<NASDtoNoImage>?> GetAllNASNoImageAsync();
        Task<IEnumerable<NASDtoNoImage>?> GetAllNasBySYSemesterAsync(int year, Semester semester);
        Task<int> GetNASIdByUsernameAsync(string username);
        Task<NasByOfficeIdListDto> GetNASByOfficeIdSYSemesterAsync(int officeId, int year, Semester semester);
        Task<List<NASSYSemOnly>> GetAllSYAndSem();
        Task<NASDtoNoImage> GetNASByNASIdSYSemesterNoImgAsync(int nasId, int year, Semester semester);
        Task<IEnumerable<NASSYSemOnly?>> GetSYSemByNASIdAsync(int nasId);
        Task<byte[]?> UploadPhotoAsync(int nasId, IFormFile file);
        Task<NASDto?> UpdateNASAsync(int nasId, NASUpdateDto nasUpdate);
        Task<bool> ChangePasswordAsync(int nasId, string currentPassword, string newPassword);
    }
}
