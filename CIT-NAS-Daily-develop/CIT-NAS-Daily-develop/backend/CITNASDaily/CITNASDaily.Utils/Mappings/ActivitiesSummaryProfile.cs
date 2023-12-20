using AutoMapper;
using CITNASDaily.Entities.Dtos.ActivitiesSummaryDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class ActivitiesSummaryProfile : Profile
    {
        public ActivitiesSummaryProfile() {
            CreateMap<ActivitiesSummary, ActivitiesSummaryCreateDto>().ReverseMap();
        }
    }
}
