using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Dtos.NASDtos
{
    public class NASPhotoUpdateDto
    {
        public int NasId { get; set; }
        public byte[]? Image { get; set; }
    }
}
