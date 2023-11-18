using PM2E2GRUPO2.Vistas;
namespace PM2E2GRUPO2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Ruta de Acceso
            Routing.RegisterRoute(nameof(CreateEmpl), typeof(CreateEmpl));
            Routing.RegisterRoute(nameof(VistaPage), typeof(VistaPage));
        }
    }
}
