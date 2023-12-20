using CITNASDaily.Entities.Models;

namespace CITNASDaily.Entities.Dtos.OfficeDtos
{
    public class OfficeDto
    {
        public int Id { get; set; }
        public string? OfficeName { get; set; }
        public string? SuperiorFirstName { get; set; }
        public string? SuperiorLastName { get; set; }
        public string? SuperiorFullName => $"{SuperiorFirstName} {SuperiorLastName}";
        public List<NAS>? NAS { get; set; }
    }
}
