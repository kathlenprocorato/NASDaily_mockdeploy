using CITNASDaily.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Dtos.BiometricLogDtos
{
    public class BiometricLogCreateDto
    {
        public int TMNo { get; set; }
        [Required]
        public int EnNo { get; set; }
        public string? Name { get; set; }
        public int GMNo { get; set; }
        public string? Mode { get; set; }
        public string? InOut { get; set; }
        public int Antipass { get; set; }
        public int ProxyWork { get; set; }
        public DateTime? DateTime { get; set; }
        [Required]
        public int NASId { get; set; }
    }
}
