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
                Routing.RegisterRoute(nameof(Page2), typeof(Page2));                //Página de autenticação
                Routing.RegisterRoute(nameof(ControlPanel), typeof(ControlPanel));  //Painel de controle
                Routing.RegisterRoute(nameof(ControleAcad), typeof(ControleAcad));  //ADMIN: controle acadêmico
                Routing.RegisterRoute(nameof(GradingManagement), typeof(GradingManagement));  //ADMIN: controle acadêmico
            }
            catch (Exception ex)
            {
                App.Logger?.WriteExceptionAsync(ex);
            }
        }
    }
}
