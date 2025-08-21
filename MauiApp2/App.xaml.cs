
using MauiApp2.Data;
using MauiApp2.Logging;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp2
{
    public partial class App : Application
    {
        //Temporary Data
        public static User? ActiveUser { get; set; }
        public static Logger Logger { get; set; } = new Logger();
        
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"[UNHANDLED EXCEPTION] {e.ExceptionObject}");
                Logger.WriteLineAsync($"[UNHANDLED EXCEPTION] {e.ExceptionObject}");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"[UNOBSERVED TASK EXCEPTION] {e.Exception}");
                Logger.WriteLineAsync($"[UNHANDLED EXCEPTION] {e.Exception}");
            };
            InitializeComponent();

            WeakReferenceMessenger.Default.Register<ErrorMessage>(this, (recipient, message) =>
            {
                // Garante que o alerta seja exibido na thread de UI
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var mp = Current?.Windows[0].Page;
                    if (mp != null)
                    {
                        await mp.DisplayAlert(message.Title, message.Value, "OK");
                    }
                });
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}