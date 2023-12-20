using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Dtos.BiometricLogDtos
{
    public class BiometricLogDto
    {
        public int No { get; set; }
        public int TMNo { get; set; }
        public int EnNo { get; set; }
        public string? Name { get; set; }
        public int GMNo { get; set; }
        public string? Mode { get; set; }
        public string? InOut { get; set; }
        public int Antipass { get; set; }
        public int ProxyWork { get; set; }
        public DateTime? DateTime { get; set; }
        public int NASId { get; set; }
    }
}
