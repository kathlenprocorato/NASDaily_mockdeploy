using CITNASDaily.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Dtos.OASDtos
{
    public class OASCreateDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
