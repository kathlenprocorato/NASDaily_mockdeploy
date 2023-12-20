using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SummaryEvaluationDtos
{
    public class SummaryEvaluationCreateDto
    {
        [Required]
        public int nasId { get; set; }
        [Required]
        public Semester Semester { get; set; }
        [Required]
        public int SchoolYear { get; set; }
        [Required]
        public float SuperiorOverallRating { get; set; }
    }
}
