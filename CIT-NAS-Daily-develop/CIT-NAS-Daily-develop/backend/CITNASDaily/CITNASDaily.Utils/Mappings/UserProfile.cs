using AutoMapper;
using CITNASDaily.Entities.Dtos.UserDtos;
using CITNASDaily.Entities.Models;

namespace CITNASDaily.Utils.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<User, UserCreateDto>()
                .ReverseMap()
                .ForMember(user => user.PasswordHash, op => op.MapFrom(dto => dto.Password))
                .ForMember(user => user.Role, op => op.MapFrom(dto => dto.Role));
        }
    }
}
