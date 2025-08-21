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

    [Flags]
    public enum StudentCourseEnum
    {
        Electrical_Engineering,
        Chemical_Engineering,
        Mechanical_Engineering,
        Mechatronics_Engineering,
        Computer_Engineering,
        Software_Engineering,
        All_Engineering = Electrical_Engineering | Chemical_Engineering | Mechanical_Engineering | Mechatronics_Engineering | Computer_Engineering | Software_Engineering
    }
}
