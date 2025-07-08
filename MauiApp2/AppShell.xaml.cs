using MauiApp2.Pages;

namespace MauiApp2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Page2), typeof(Page2));                //Página de autenticação
            Routing.RegisterRoute(nameof(ControlPanel), typeof(ControlPanel));  //Painel de controle
            Routing.RegisterRoute(nameof(ControleAcad), typeof(ControleAcad));  //ADMIN: controle acadêmico
        }
    }
}
