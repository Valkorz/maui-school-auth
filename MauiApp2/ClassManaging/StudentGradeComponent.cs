using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public struct ComponentApplicationInfo
    {
        public string Identification { get; set; }
        public string Classroom { get; set; }
        public Weekdays Day { get; set; }
        public TimeSpan Period { get; set; }
    }
    
    public class StudentGradeComponent
    {
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
            byte[] randBytes = new byte[6];
            rand.NextBytes(randBytes);

            // Add some default component application info
            AvailableInfo.Add(new ComponentApplicationInfo
            { 
                Identification = $"TIN00{randBytes}", //randomized indentifier
                Classroom = "C37",
                Day = Weekdays.Monday,
                Period = new TimeSpan(19, 0, 0)
            });

            //random stuff just for testing purposes. Should be removed later.
            
        }
    }
}
