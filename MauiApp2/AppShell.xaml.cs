using MauiApp2.Pages;

namespace MauiApp2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            try
            {
                Routing.RegisterRoute(nameof(Page2), typeof(Page2));                            //Login page
                Routing.RegisterRoute(nameof(ControlPanel), typeof(ControlPanel));              //User Control Panel (main page)
                Routing.RegisterRoute(nameof(ControleAcad), typeof(ControleAcad));              //ADMIN: User control
                Routing.RegisterRoute(nameof(GradingManagement), typeof(GradingManagement));    //ADMIN: Scheduling control
                Routing.RegisterRoute(nameof(Settings), typeof(Settings));                      //App settings
            }
            catch (Exception ex)
            {
                App.Logger?.WriteExceptionAsync(ex);
            }
        }
    }
}
