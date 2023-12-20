using CITNASDaily.Entities.Models;
using System.ComponentModel.DataAnnotations;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SuperiorEvaluationRatingDto
{
    public class SuperiorEvaluationRatingCreateDto
    {


        [Required]
        public int NASId { get; set; }
        [Required]
        public int SchoolYear { get; set; }
        [Required]
        public Semester Semester { get; set; }
        [Required]
        public int AttendanceAndPunctuality { get; set; }
        [Required]
        public int QualOfWorkOutput { get; set; }
        [Required]
        public int QuanOfWorkOutput { get; set; }
        [Required]
        public int AttitudeAndWorkBehaviour { get; set; }
        [Required]
        public int OverallAssessment { get; set; }
    }
}
