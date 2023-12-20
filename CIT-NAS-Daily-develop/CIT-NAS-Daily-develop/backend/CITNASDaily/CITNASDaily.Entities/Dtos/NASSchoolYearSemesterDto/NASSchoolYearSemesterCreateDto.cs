using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.SchoolYearDto
{
    public class NASSchoolYearSemesterCreateDto
    {
        public int Year { get; set; }
        public Semester Semester { get; set; }
    }
}