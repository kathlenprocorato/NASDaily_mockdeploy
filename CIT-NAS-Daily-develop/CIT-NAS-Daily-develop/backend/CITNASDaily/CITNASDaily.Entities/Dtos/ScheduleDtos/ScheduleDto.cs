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
    public class ScheduleDto
    {
        public DaysOfTheWeek DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool BrokenSched { get; set; }
        public float TotalHours { get; set; }
    }
}
