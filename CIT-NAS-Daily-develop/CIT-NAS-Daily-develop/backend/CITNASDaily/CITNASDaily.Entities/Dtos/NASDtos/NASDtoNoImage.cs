using CITNASDaily.Entities.Dtos.SchoolYearDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.NASDtos
{
    public class NASDtoNoImage
    {
        public int Id { get; set; }
        public string? StudentIDNo { get; set; }
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public List<NASSchoolYearSemesterCreateDto>? SYSem { get; set; }
        public int OfficeId { get; set; }
        public string? OfficeName { get; set; }
        public int? EnNo { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Course { get; set; }
        public int? YearLevel { get; set; }
        public int? UnitsAllowed { get; set; }
        public DateTime? DateStarted { get; set; }
    }
}
