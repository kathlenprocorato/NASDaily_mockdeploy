using CITNASDaily.Entities.Dtos.ValidationDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Services.Contracts
{
    public interface IValidationService
    {
        Task<Validation?> CreateValidationAsync(ValidationCreateDto validationCreateDto);
        Task<IEnumerable<Validation>?> GetAllValidationsAsync();
        Task<ValidationDto?> GetValidationByIdAsync(int validationId);
        Task<IEnumerable<Validation>?> GetValidationByNasIdAsync(int nasId);
        Task<Validation?> UpdateValidationAsync(ValidationUpdateDto validation, int validationId);
    }
}
