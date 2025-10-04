using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MauiApp2.ClassManaging
{
    public enum Weekdays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    //Define a specific time of the week where the class takes place.
    public class ComponentApplicationInfo
    {
        [Key]
        public int Id { get; set; }
        public string Identification { get; set; } = string.Empty;
        public string Classroom { get; set; } = string.Empty;
        public Weekdays Day { get; set; }
        public TimeSpan PeriodStart { get; set; }
        public TimeSpan PeriodEnd { get; set; }

        // Chave estrangeira para StudentGradeComponent
        public int StudentGradeComponentId { get; set; }
        public string StudentGradeComponentCode { get; set; } = string.Empty;
    }
    
    public class StudentGradeComponent
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Semester { get; set; }

        public readonly int Credits = 4000;

        public StudentCourseEnum TargetCourses { get; set; }
        public List<ComponentApplicationInfo> AvailableInfo { get; set; } = new List<ComponentApplicationInfo>();
        public StudentGradeComponent(string name, string description, int semester) 
        { 
            Name = name;
            Description = description;
            Semester = semester;

            var rand = new Random(DateTime.Now.Second); //generate a random instance
            string hexChars = "0123456789ABCDEF";
            byte[] randBytes = new byte[6];
            rand.NextBytes(randBytes);

            for (int i = 0; i < randBytes.Length; i++)
            {
                Code += hexChars[randBytes[i] % hexChars.Length];
            }

            //// Add some default component application info
            //AvailableInfo.Add(new ComponentApplicationInfo
            //{
            //    Identification = "TIN00" + Code, //randomized indentifier
            //    Classroom = "C37",
            //    Day = Weekdays.Monday,
            //    PeriodStart = new TimeSpan(19, 0, 0),
            //    PeriodEnd = new TimeSpan(20, 40, 0)
            //});           
        }
    }
}
