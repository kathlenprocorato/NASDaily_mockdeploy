using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CITNASDaily.Entities.Models
{
    public class Superior
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

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
        public string? OfficeName { get; set; }
    }
}
