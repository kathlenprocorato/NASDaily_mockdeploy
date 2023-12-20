using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.ActivitiesSummaryDtos
{
    public class ActivitiesSummaryCreateDto
    {
        public string? ActivitiesOfTheDay { get; set; }
        public string? SkillsLearned { get; set; }
        public string? ValuesLearned { get; set; }
    }
}
