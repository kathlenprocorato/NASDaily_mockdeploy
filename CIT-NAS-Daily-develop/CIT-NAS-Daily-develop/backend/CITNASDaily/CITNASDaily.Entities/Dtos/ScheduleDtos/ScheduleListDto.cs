using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.ScheduleDtos
{
    public class ScheduleListDto
    {
        public int NASId { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public List<ScheduleDto>? Schedules { get; set; }
    }
}
