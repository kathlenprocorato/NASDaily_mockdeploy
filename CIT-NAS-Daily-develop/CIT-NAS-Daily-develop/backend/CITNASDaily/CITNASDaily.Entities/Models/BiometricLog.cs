using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Models
{
    public class BiometricLog
    {
        [Key]
        public int No { get; set; }
        public int TMNo { get; set; }
        public int EnNo { get; set; }
        public string? Name { get; set; }
        public int GMNo { get; set; }
        public string? Mode { get; set; }
        public string? InOut { get; set; }
        public int Antipass { get; set; }
        public int ProxyWork { get; set;}
        public DateTime? DateTime { get; set; }
        public int NASId { get; set; } 
        public NAS NAS { get; set; }
    }
}
