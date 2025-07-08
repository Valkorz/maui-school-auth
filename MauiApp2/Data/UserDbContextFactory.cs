using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Data
{
    class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsbuilder = new DbContextOptionsBuilder<UserDbContext>();

            optionsbuilder.UseSqlite("Filename=Users.db3");
            return new UserDbContext(optionsbuilder.Options);
        }
    }
}
