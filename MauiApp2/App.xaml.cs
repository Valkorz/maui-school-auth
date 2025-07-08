using MauiApp2.Data;

namespace MauiApp2
{
    public partial class App : Application
    {
        //Temporary Data
        public static User? ActiveUser { get; set; }
        
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}