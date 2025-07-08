using Microsoft.Extensions.Logging;
using System;
using MauiApp2.Data;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MauiApp2
{
    //The main program file that builds the software
    public static class MauiProgram
    {
        public const string DbFileName = "Users";
       
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Add db context class with builder

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, $"{DbFileName}.db3");
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));
            builder.Services.AddScoped<UserControl>();  //Dependency injection for UserControl class (will fill every page constructor parameter)

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
