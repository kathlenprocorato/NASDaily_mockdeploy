using System.ComponentModel.DataAnnotations;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Models
{
    /// <summary>
    /// This is the class for the Validation Model
    /// </summary>
    public class Validation
    {
        [Key]
        public int Id { get; set; }
        [Required]
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
