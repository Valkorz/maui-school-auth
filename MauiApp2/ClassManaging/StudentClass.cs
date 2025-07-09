using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MauiApp2.ClassManaging
{
    /* StudentClass implementation for managing the user's grading and course information. Each course should have it's
     * respective per-semester components, duration and student performance for each component.
    */
    
    public enum StudentClassEnum
    {
        Electrical_Engineering,
        Chemical_Engineering,
        Mechanical_Engineering,
        Mechatronics_Engineering,
        Computer_Engineering,
        Software_Engineering,
    }

    public struct StudentClassData
    {
        public string Name { get; set; }
        public int LengthSemesters { get; set; }
    }

    public struct StudentClass
    {
        public static readonly Dictionary<StudentClassEnum, StudentClassData> SClassData = new()
        {
            { StudentClassEnum.Electrical_Engineering,      new StudentClassData{ Name = "Electrical Engineering" , LengthSemesters = 10} },
            { StudentClassEnum.Chemical_Engineering,        new StudentClassData{ Name = "Chemical Engineering" , LengthSemesters = 10} },
            { StudentClassEnum.Mechanical_Engineering,      new StudentClassData{ Name = "Mechanical Engineering" , LengthSemesters = 10} },
            { StudentClassEnum.Mechatronics_Engineering,    new StudentClassData{ Name = "Mechatronics Engineering" , LengthSemesters = 10} },
            { StudentClassEnum.Computer_Engineering,        new StudentClassData{ Name = "Computer Engineering" , LengthSemesters = 10} },
            { StudentClassEnum.Software_Engineering,        new StudentClassData{ Name = "Software Engineering" , LengthSemesters = 10} }
        };

        public StudentClassEnum SClass { get; set; }
        


    }
}
