namespace CITNASDaily.Entities.Dtos.SuperiorDtos
{
    public class SuperiorDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public Guid? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? OfficeName { get; set; }
    }
}
