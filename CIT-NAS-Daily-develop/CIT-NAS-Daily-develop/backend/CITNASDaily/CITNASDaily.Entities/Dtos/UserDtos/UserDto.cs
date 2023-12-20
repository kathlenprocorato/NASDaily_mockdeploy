using CITNASDaily.Entities.Models;

namespace CITNASDaily.Entities.Dtos.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
