using CITNASDaily.Entities.Dtos.ScheduleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.NASDtos
{
    public class NasByOfficeIdListDto
    {
        public int OfficeId { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public List<NASDtoNoImage>? NASEntries { get; set; }
    }
}
