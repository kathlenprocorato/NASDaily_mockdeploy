using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.ScheduleDtos
{
    public class ScheduleCreateDto
    {
        [Required]
        public int NASId { get; set; }
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
