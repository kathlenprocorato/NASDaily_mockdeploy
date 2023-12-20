using AutoMapper;
using CITNASDaily.Entities.Dtos.ValidationDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Services.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IValidationRepository _validationRepository;
        private readonly IMapper _mapper;

        public ValidationService(IValidationRepository validationRepository, IMapper mapper)
        {
            _validationRepository = validationRepository;
            _mapper = mapper;
        }

        public async Task<Validation?> CreateValidationAsync(ValidationCreateDto validationCreateDto)
        {
            var val = _mapper.Map<Validation>(validationCreateDto);

            var createdVal = await _validationRepository.CreateValidationAsync(val);

            if(createdVal != null)
            {
                return createdVal;
            }
            return null;
        }

        public async Task<IEnumerable<Validation>?> GetAllValidationsAsync()
        {
            return await _validationRepository.GetAllValidationsAsync();
        }

        public async Task<ValidationDto?> GetValidationByIdAsync(int validationId)
        {
            var validation = await _validationRepository.GetValidationByIdAsync(validationId);
            return _mapper.Map<ValidationDto>(validation);
        }

        public async Task<IEnumerable<Validation>?> GetValidationByNasIdAsync(int nasId)
        {
            return await _validationRepository.GetValidationByNasIdAsync(nasId);
        }

        public async Task<Validation?> UpdateValidationAsync(ValidationUpdateDto validationUpdateDto, int validationId)
        {
            var validation = _mapper.Map<Validation>(validationUpdateDto);
            return await _validationRepository.UpdateValidationAsync(validation, validationId);
        }
    }
}
