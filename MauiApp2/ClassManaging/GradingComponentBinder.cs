using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2.Data;

namespace MauiApp2.ClassManaging
{
    public class GradingComponentBinder(string name, string code, ComponentApplicationInfo info)
    {
        [Key]
        public int Id { get; set; } //PK
        public string Name { get; set; } = name;
        public string Code { get; set; } = code;

        //Info
        public string Classroom { get; set; } = info.Classroom;
        public Weekdays Day { get; set; } = info.Day;
        public TimeSpan PeriodStart { get; set; } = info.PeriodStart;
        public TimeSpan PeriodEnd { get; set; } = info.PeriodEnd;

        //Foregin Key
        public int UserId { get; set; }
        public User User { get; set; } = new User();

        public GradingComponentBinder() : this(string.Empty, string.Empty, new ComponentApplicationInfo()) { }
    }
}
