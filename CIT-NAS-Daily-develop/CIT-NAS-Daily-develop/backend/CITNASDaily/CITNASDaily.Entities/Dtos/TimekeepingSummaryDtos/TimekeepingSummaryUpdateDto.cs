using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.TimekeepingSummaryDtos
{
    public class TimekeepingSummaryUpdateDto
    {
        public int? Excused { get; set; }
        public int? Unexcused { get; set; }
        public double? MakeUpDutyHours { get; set; }
    }
}
