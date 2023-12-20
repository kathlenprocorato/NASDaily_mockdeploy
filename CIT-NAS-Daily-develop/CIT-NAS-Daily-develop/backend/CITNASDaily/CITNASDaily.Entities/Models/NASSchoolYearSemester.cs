using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Models
{
    public class NASSchoolYearSemester
    {
        [Key]
        public int Id { get; set; }
        public int NASId { get; set; }
        public int Year { get; set; }
        public Semester Semester { get; set; }
    }
}