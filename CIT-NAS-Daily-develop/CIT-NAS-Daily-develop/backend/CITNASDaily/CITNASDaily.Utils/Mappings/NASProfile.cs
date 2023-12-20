using AutoMapper;
using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.SchoolYearDto;
using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class NASProfile : Profile
    {
        public NASProfile()
        {
            CreateMap<NAS, NASDto>().ReverseMap();
            CreateMap<NAS, NASDtoNoImage>().ReverseMap();

            CreateMap<NAS, NASCreateDto>()
                .ReverseMap()
                .ForMember(n => n.Username, op => op.MapFrom(dto => dto.Username))
                .ForMember(n => n.FirstName, op => op.MapFrom(dto => dto.FirstName))
                .ForMember(n => n.MiddleName, op => op.MapFrom(dto => dto.MiddleName))
                .ForMember(n => n.LastName, op => op.MapFrom(dto => dto.LastName))
                .ForMember(n => n.Gender, op => op.MapFrom(dto => dto.Gender))
                .ForMember(n => n.BirthDate, op => op.MapFrom(dto => dto.BirthDate))
                .ForMember(n => n.Course, op => op.MapFrom(dto => dto.Course))
                .ForMember(n => n.YearLevel, op => op.MapFrom(dto => dto.YearLevel))
                .ForMember(n => n.UnitsAllowed, op => op.MapFrom(dto => dto.UnitsAllowed))
                .ForMember(n => n.OfficeId, op => op.MapFrom(dto => dto.OfficeId))
                .ForMember(n => n.EnNo, op => op.MapFrom(dto => dto.EnNo))
                .ForMember(n => n.DateStarted, op => op.MapFrom(dto => dto.DateStarted));

            CreateMap<NAS, NASPhotoUpdateDto>().ReverseMap();
            CreateMap<NAS, NASUpdateDto>().ReverseMap();
            CreateMap<NAS, NASSchoolYearSemesterCreateDto>().ReverseMap();
        }
    }
}
