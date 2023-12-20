using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Models
{
    public class OAS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public User? User { get; set; }
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
