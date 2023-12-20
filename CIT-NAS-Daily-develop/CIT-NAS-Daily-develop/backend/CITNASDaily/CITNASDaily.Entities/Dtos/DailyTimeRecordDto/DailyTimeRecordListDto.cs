using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.DailyTimeRecordDto
{
    public class DailyTimeRecordListDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public List<DailyTimeRecordDto>? DailyTimeRecords { get; set;}
    }
}
