using AutoMapper;
using CITNASDaily.Entities.Dtos.OASDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class OASProfile : Profile
    {
        public OASProfile()
        {

            CreateMap<OAS, OASDto>().ReverseMap();

            CreateMap<OAS, OASCreateDto>()
                .ReverseMap()
                .ForMember(oas => oas.Username, op => op.MapFrom(dto => dto.Username))
                .ForMember(oas => oas.FirstName, op => op.MapFrom(dto => dto.FirstName))
                .ForMember(oas => oas.MiddleName, op => op.MapFrom(dto => dto.MiddleName))
                .ForMember(oas => oas.LastName, op => op.MapFrom(dto => dto.LastName));
        }
    }
}
