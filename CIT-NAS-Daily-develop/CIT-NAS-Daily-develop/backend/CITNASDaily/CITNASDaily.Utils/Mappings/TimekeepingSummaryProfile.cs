using AutoMapper;
using CITNASDaily.Entities.Dtos.TimekeepingSummaryDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class TimekeepingSummaryProfile : Profile
    {
        public TimekeepingSummaryProfile() {
            CreateMap<TimekeepingSummary, TimekeepingSummaryCreateDto>().ReverseMap();
            CreateMap<TimekeepingSummary, TimekeepingSummaryUpdateDto>().ReverseMap();
        }
    }
}
