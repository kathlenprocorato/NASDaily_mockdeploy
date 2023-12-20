using AutoMapper;
using CITNASDaily.Entities.Dtos.DailyTimeRecordDto;
using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class DailyTimeRecordProfile : Profile
    {
        public DailyTimeRecordProfile() 
        {
            CreateMap<DailyTimeRecord, DailyTimeRecordDto>().ReverseMap();
        }
    }
}
