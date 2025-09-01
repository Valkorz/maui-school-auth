using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2.Data;
using MauiApp2.ClassManaging;

namespace MauiApp2.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime TimeOfCreation { get; set; }
        public string Email { get; set; } = string.Empty;
        public User.UserPermissions Permissions { get; set; }
    }
}
