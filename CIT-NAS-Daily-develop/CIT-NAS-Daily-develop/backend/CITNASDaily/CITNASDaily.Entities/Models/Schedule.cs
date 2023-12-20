using System.ComponentModel.DataAnnotations;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NASId { get; set; }
        public NAS? NAS { get; set; }
        [Required]
        public DaysOfTheWeek DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Required]
        public bool BrokenSched { get; set; }
        [Required]
        public float TotalHours { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
    }
}
