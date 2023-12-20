using AutoMapper;
using CITNASDaily.Entities.Dtos.BiometricLogDtos;
using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;

namespace CITNASDaily.Services.Services
{
    public class BiometricLogService : IBiometricLogService
    {
        private readonly IBiometricLogRepository _logRepository;
        private readonly INASRepository _nasRepository;
        private readonly IMapper _mapper;

        public BiometricLogService(INASRepository nasRepository, IBiometricLogRepository logRepository, IMapper mapper)
        {
            _nasRepository = nasRepository;
            _mapper = mapper;
            _logRepository = logRepository;
        }
        public async Task<BiometricLogDto?> CreateLogAsync(int nasId, BiometricLogCreateDto logCreate)
        {
            var log = _mapper.Map<BiometricLog>(logCreate);
            log.NASId = nasId;
            var createdLog = await _logRepository.CreateLogAsync(log);

            return _mapper.Map<BiometricLogDto>(createdLog);
        }

        public async Task<IEnumerable<BiometricLogDto?>> GetNASLogsAsync(int nasId)
        {
            var logs = await _logRepository.GetNASLogsAsync(nasId);

            return _mapper.Map<IEnumerable<BiometricLogDto>>(logs);
        }
    }
}
