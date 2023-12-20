using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.ValidationDtos
{
    public class ValidationCreateDto
    {
        public int NasId { get; set; }
        public byte[]? NasLetter { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
    }
}
