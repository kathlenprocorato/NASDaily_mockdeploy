using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.ValidationDtos
{
    public class ValidationDto
    {
        public int Id { get; set; }
        public int NasId { get; set; }
        public byte[]? NasLetter { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public Semester Semester { get; set; }
        public int SchoolYear { get; set; }
        public ValidationStatus ValidationStatus { get; set; }
        public int MakeUpHours { get; set; }
    }
}
