using MauiApp2.ClassManaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Models
{
    public class SchedulingData
    {
        public string GradeName { get; set; } = string.Empty;
        public string GradeIdentification { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public string Classroom { get; set; } = string.Empty;
        public Weekdays Day { get; set; }
        public TimeSpan PeriodStart { get; set; }
        public TimeSpan PeriodEnd { get; set; }

        public GradingComponentBinder ToComponentBinder()
        {
            return new GradingComponentBinder
            {
                Name = GradeName,
                Classroom = Classroom,
                Day = Day,
                PeriodStart = PeriodStart,
                PeriodEnd = PeriodEnd,
                Code = Identification,
                ComponentCode = GradeIdentification
            };
        }

        public SchedulingData() { }
        public SchedulingData(GradingComponentBinder binder)
        {
            GradeName = binder.Name;
            Classroom = binder.Classroom;
            Day = binder.Day;
            PeriodStart = binder.PeriodStart;
            PeriodEnd = binder.PeriodEnd;
            Identification = binder.Code;
            GradeIdentification = binder.ComponentCode;
        }
    }
}
