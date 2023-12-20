using AutoMapper;
using CITNASDaily.Entities.Dtos.SchoolYearDto;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class NASSchoolYearProfile : Profile
    {
        public NASSchoolYearProfile()
        {
            CreateMap<NASSchoolYearSemester, NASSchoolYearSemesterCreateDto>().ReverseMap();
        }
    }
}