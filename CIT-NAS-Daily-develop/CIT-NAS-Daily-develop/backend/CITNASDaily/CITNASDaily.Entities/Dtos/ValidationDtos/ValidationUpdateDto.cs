using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Dtos.ValidationDtos
{
    public class ValidationUpdateDto
    {
        public ValidationStatus ValidationStatus { get; set; }
        public int MakeUpHours { get; set; }
    }
}
