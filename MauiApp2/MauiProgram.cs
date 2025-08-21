using Microsoft.Extensions.Logging;
using System;
using MauiApp2.Data;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui;

namespace MauiApp2
{
    //The main program file that builds the software
    public static class MauiProgram
    {
        public const string DbFileName = "Users";
       
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            try
            {
                builder
                    .UseMauiApp<App>()
                    .UseMauiCommunityToolkit()
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


            }
            catch (Exception ex)
            {
                App.Logger?.WriteExceptionAsync(ex);
                throw;
            }

            var app = builder.Build();

    
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                dbContext.Database.Migrate();
            }
            return app;
        }
    }
}
