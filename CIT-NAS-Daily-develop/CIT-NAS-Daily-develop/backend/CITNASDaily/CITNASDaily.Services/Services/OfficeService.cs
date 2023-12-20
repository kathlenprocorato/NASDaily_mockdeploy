using AutoMapper;
using CITNASDaily.Entities.Dtos.OfficeDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Services.Contracts;

namespace CITNASDaily.Services.Services
{
    public class OfficeService : IOfficeService
    {
        public readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;
        public OfficeService(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<Office?> CreateOfficeAsync(OfficeCreateDto office)
        {
            var of = _mapper.Map<Office>(office);
            var createdOffice = await _officeRepository.CreateOfficeAsync(of);

            if (createdOffice != null)
            {
                return createdOffice;
            }
            return null;
        }

        public async Task<Office?> GetOfficeByIdAsync(int id)
        {
            return await _officeRepository.GetOfficeByIdAsync(id);
        }

        public async Task<Office?> GetOfficeByNASIdAsync(int nasId)
        {
            return await _officeRepository.GetOfficeByNASIdAsync(nasId);
        }

        public async Task<Office?> GetOfficeBySuperiorIdAsync(int superiorId)
        {
            return await _officeRepository.GetOfficeBySuperiorIdAsync(superiorId);
        }

        public async Task<IEnumerable<Office?>> GetOfficesAsync()
        {
            return await _officeRepository.GetOfficesAsync();
        }

        public async Task<OfficeDto?> UpdateOfficeAsync(OfficeUpdateDto office)
        {
            var of = _mapper.Map<Office>(office);
            var updatedOffice = await _officeRepository.UpdateOfficeAsync(of);

            if (updatedOffice == null)
            {
                return null;
            }
            
            return _mapper.Map<OfficeDto>(updatedOffice);
        }
    }
}
