using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.Entities.Models
{
    public class NASSYSemOnly
    {
        public int Year { get; set; }
        public Semester Semester { get; set; }
    }
}
