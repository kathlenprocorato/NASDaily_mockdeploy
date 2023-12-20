using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Dtos.SuperiorDtos
{
    public class SuperiorCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        public string? Username { get; set; }
    }
}
