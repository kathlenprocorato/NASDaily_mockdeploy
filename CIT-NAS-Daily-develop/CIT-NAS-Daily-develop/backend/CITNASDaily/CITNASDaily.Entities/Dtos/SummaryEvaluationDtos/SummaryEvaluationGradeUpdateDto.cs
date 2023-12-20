using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SummaryEvaluationDtos
{
    public class SummaryEvaluationGradeUpdateDto
    {
        public int nasId { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
    }
}
