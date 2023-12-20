using CITNASDaily.Entities.Dtos.SchoolYearDto;
using CITNASDaily.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.NASDtos
{
    public class NASUpdateDto
    {
        public int? OfficeId { get; set; }
        public int? YearLevel { get; set; }
        public string? Course { get; set; }
        public List<NASSchoolYearSemesterCreateDto>? SYSem { get; set; }
        public int? UnitsAllowed { get; set; }
    }
}