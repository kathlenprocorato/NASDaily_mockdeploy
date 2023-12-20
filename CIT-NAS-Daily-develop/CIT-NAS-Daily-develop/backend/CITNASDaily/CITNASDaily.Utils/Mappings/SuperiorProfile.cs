using AutoMapper;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class SuperiorProfile : Profile
    {
        public SuperiorProfile()
        {

            CreateMap<Superior, SuperiorDto>().ReverseMap();

            CreateMap<Superior, SuperiorCreateDto>()
                .ReverseMap()
                .ForMember(superior => superior.Username, op => op.MapFrom(dto => dto.Username))
                .ForMember(superior => superior.FirstName, op => op.MapFrom(dto => dto.FirstName))
                .ForMember(superior => superior.LastName, op => op.MapFrom(dto => dto.LastName));
        }
    }
}
    