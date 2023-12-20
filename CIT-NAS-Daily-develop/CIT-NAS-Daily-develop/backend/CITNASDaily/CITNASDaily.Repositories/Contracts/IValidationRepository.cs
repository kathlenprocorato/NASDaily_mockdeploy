using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Repositories.Contracts
{
    public interface IValidationRepository
    {
        Task<Validation?> CreateValidationAsync(Validation validation);
        Task<IEnumerable<Validation>?> GetAllValidationsAsync();
        Task<Validation?> GetValidationByIdAsync(int validationId);
        Task<IEnumerable<Validation>?> GetValidationByNasIdAsync(int nasId);
        Task<Validation?> UpdateValidationAsync(Validation validation, int validationId);
    }
}
