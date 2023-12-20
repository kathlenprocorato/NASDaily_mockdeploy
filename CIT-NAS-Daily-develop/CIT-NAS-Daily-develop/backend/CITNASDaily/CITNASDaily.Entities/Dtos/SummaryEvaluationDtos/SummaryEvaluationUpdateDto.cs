using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SummaryEvaluationDtos
{
    public class SummaryEvaluationUpdateDto
    {
        public int nasId { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public bool EnrollmentAllowed { get; set; }
        public bool AllCoursesPassed { get; set; }
        public int NoOfCoursesFailed { get; set; }
        public string? Responded { get; set; }
    }
}
