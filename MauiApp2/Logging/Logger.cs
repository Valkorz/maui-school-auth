using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Logging
{
    public class Logger
    {
        public string DirectoryPath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FullPath => System.IO.Path.Combine(DirectoryPath, FileName);
        public Logger()
        {
            //Generate file name
            //DirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            //string dateString = DateTime.Now.ToString()
            //    .Replace("/", "-")
            //    .Replace("\\", "-")
            //    .Replace(":", ".");
            //Directory.CreateDirectory(Path.Combine(DirectoryPath, "Logs"));
            //FileName = $"Logs/ExecutionLog_{dateString}";

#if WINDOWS
            DirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Maui_Logs");
#elif ANDROID
            // Obtém o diretório público de Documentos no Android
            // Requer permissão de armazenamento!
            string? androidDocsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
            if (androidDocsPath != null) 
            {
                DirectoryPath = Path.Combine( androidDocsPath
                ,
                "Maui_Logs");
            }
#else
            // Fallback para iOS, MacCatalyst, etc.
            DirectoryPath = Path.Combine(FileSystem.AppDataDirectory, "Maui_Logs");
#endif
            Directory.CreateDirectory(DirectoryPath);


            string dateString = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
            FileName = $"ExecutionLog_{dateString}.txt";
            Initialize();
        }

        public async void Initialize()
        {                                                                                               //
            string text = @"############# --              LOGGING STARTED          -- ##################";
            await WriteLineAsync(text);
            await WriteLineAsync($"Created logging file at: {FullPath}");
        }

        public async Task WriteLineAsync(string text)
        {
            using var writer = new StreamWriter(FullPath, append: true);
            await writer.WriteLineAsync($"[ {DateTime.Now.TimeOfDay} ]: {text}");
        }

        public async Task WriteExceptionAsync(Exception ex, bool interrupt = false)
        {
            using var writer = new StreamWriter($"{FullPath}", append: true);

            //Exception format
            await writer.WriteLineAsync("\n");
            string text = 
@$"############# -- EXCEPTION AT {DateTime.Now.TimeOfDay}-- ################
{ex.Message}
############################################################################";

            await writer.WriteLineAsync(text);
            await writer.WriteLineAsync("\n");

            if (interrupt)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage("Erro Inesperado", ex.Message));
            }
        }
        
    }
}
