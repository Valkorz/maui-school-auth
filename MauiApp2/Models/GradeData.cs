using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Models
{
    public class GradeData
    {
        public string GradeCode { get; set; } = string.Empty;
        public string GradeName { get; set;} = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int GradeCredits { get; set; }
        public int Semester { get; set; }
    }
}
