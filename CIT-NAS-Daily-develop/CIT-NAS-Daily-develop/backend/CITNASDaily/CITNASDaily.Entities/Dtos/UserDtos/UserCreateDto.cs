using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Dtos.UserDtos
{
    public class UserCreateDto
    {

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
       
        [Required]
        public string Role { get; set; }
    }
}
