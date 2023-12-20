using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITNASDaily.Entities.Enums
{
    public class Enums
    {
        public enum Semester
        {
            [Description("1st Semester")]
            FirstSemester,
            [Description("2nd Semester")]
            SecondSemester,
            [Description("Summer")]
            Summer
        }

        public enum DaysOfTheWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday
        }

        public enum ValidationStatus
        {
            [Description("Pending")]
            Pending,
            [Description("Excused")]
            Excused,
            [Description("Unexcused")]
            Unexcused,
            [Description("For Make Up Duty")]
            ForMakeUpDuty,
            [Description("Approved")]
            Approved,
            [Description("Disapproved")]
            Disapproved,
            [Description("Warning")]
            Warning,
            [Description("Last Warning")]
            LastWarning,
            [Description("Report To Office")]
            ReportToOffice
        }
    }
}
