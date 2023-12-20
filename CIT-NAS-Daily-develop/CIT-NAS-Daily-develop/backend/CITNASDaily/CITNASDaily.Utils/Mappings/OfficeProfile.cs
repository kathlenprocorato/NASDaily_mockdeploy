using AutoMapper;
using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.OfficeDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<Office, OfficeDto>().ReverseMap();

            CreateMap<Office, OfficeCreateDto>()
                .ReverseMap()
                .ForMember(o => o.OfficeName, op => op.MapFrom(dto => dto.OfficeName));

            CreateMap<Office, OfficeUpdateDto>().ReverseMap();
        }
    }
}
