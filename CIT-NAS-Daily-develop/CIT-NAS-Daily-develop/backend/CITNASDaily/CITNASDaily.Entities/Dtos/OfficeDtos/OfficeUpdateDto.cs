using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.OfficeDtos
{
    public class OfficeUpdateDto
    {
        public int Id { get; set; }
        public string? SuperiorFirstName { get; set; }
        public string? SuperiorLastName { get; set; }
    }
}
