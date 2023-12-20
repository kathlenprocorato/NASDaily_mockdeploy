using AutoMapper;
using CITNASDaily.Entities.Dtos.BiometricLogDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class BiometricLogProfile : Profile
    {
        public BiometricLogProfile()
        {
            CreateMap<BiometricLog, BiometricLogDto>().ReverseMap();

            CreateMap<BiometricLog, BiometricLogCreateDto>()
                .ReverseMap()
                .ForMember(b => b.TMNo, op => op.MapFrom(dto => dto.TMNo))
                .ForMember(b => b.GMNo, op => op.MapFrom(dto => dto.GMNo))
                .ForMember(b => b.EnNo, op => op.MapFrom(dto => dto.EnNo))
                .ForMember(b => b.Name, op => op.MapFrom(dto => dto.Name))
                .ForMember(b => b.Mode, op => op.MapFrom(dto => dto.Mode))
                .ForMember(b => b.InOut, op => op.MapFrom(dto => dto.InOut))
                .ForMember(b => b.Antipass, op => op.MapFrom(dto => dto.Antipass))
                .ForMember(b => b.ProxyWork, op => op.MapFrom(dto => dto.ProxyWork))
                .ForMember(b => b.DateTime, op => op.MapFrom(dto => dto.DateTime))
                .ForMember(b => b.NASId, op => op.MapFrom(dto => dto.NASId));
        }
    }
}
