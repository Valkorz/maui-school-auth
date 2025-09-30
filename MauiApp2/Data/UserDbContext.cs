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
        public DbSet<GradingComponentBinder> GradingComponentBinders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GradingComponents)
                .WithOne(gcb => gcb.User)
                .HasForeignKey(gcb => gcb.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentGradeComponent>()
                .Property(e => e.TargetCourses)
                .HasConversion<int>();

            modelBuilder.Entity<StudentGradeComponent>()
                .HasMany(e => e.AvailableInfo)
                .WithOne()
                .HasForeignKey(info => info.StudentGradeComponentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
