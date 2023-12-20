using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.TimekeepingSummaryDtos
{
    public class TimekeepingSummaryCreateDto
    {
        public int? Excused { get; set; }
        public int? Unexcused { get; set; }
        public int? FailedToPunch { get; set; }
        public int? LateOver10Mins { get; set; }
        public int? LateOver45Mins { get; set; }
        public double? MakeUpDutyHours { get; set; }
    }
}
