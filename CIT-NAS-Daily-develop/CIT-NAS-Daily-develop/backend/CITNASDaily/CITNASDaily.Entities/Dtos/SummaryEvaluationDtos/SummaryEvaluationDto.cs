using System.ComponentModel.DataAnnotations;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SummaryEvaluationDtos
{
    public class SummaryEvaluationDto
    {
        public int Id { get; set; }
        public int nasId { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public float SuperiorOverallRating { get; set; }
        public string? TimekeepingStatus { get; set; }
        public bool EnrollmentAllowed { get; set; }
        public int? UnitsAllowed { get; set; }
        public bool AllCoursesPassed { get; set; }
        public int NoOfCoursesFailed { get; set; }
        public string? Responded { get; set; }
    }
}
