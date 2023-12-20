using AutoMapper;
using CITNASDaily.Entities.Dtos.ValidationDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class ValidationProfile : Profile
    {
        public ValidationProfile() 
        {
            CreateMap<Validation, ValidationDto>().ReverseMap();
            CreateMap<Validation, ValidationCreateDto>().ReverseMap();
            CreateMap<Validation, ValidationUpdateDto>().ReverseMap();
        }
    }
}
