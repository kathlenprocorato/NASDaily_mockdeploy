using AutoMapper;
using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Utils.Mappings
{
    public class SummaryEvaluationProfile : Profile
    {
        public SummaryEvaluationProfile() {
            CreateMap<SummaryEvaluation, SummaryEvaluationDto>().ReverseMap();
            CreateMap<SummaryEvaluation, SummaryEvaluationCreateDto>().ReverseMap();
            CreateMap<SummaryEvaluation, SummaryEvaluationUpdateDto>().ReverseMap();
            CreateMap<SummaryEvaluation, SummaryEvaluationGradeUpdateDto>().ReverseMap();
        }
    }
}
