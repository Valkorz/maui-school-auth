using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MauiApp2.ClassManaging;

namespace MauiApp2.Data
{
    public partial class UserDbContext(DbContextOptions<UserDbContext> opts) : DbContext(opts)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<StudentGradeComponent> Components { get; set; }
    }
}
