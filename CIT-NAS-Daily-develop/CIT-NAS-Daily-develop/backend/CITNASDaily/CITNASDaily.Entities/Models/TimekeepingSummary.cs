using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Models
{
    public class TimekeepingSummary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NASId { get; set; }
        [Required]
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public int? Excused { get; set; }
        public int? Unexcused { get; set; }
        public int? FailedToPunch { get; set; }
        public int? LateOver10Mins { get; set; }
        public int? LateOver45Mins { get; set; }
        public double? MakeUpDutyHours { get; set; }
        public string? TimekeepingStatus { get; set; }
    }
}
