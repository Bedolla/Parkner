using Parkner.Core.Constants;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using Syncfusion.Licensing;
using System.Globalization;
using Xamarin.Forms;

namespace Parkner.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            CultureInfo cultura = CultureInfo.CreateSpecificCulture("es-MX");
            CultureInfo.DefaultThreadCurrentUICulture = cultura;
            CultureInfo.DefaultThreadCurrentCulture = cultura;

            SyncfusionLicenseProvider.RegisterLicense(Licencias.Xamarin);

            Dependencia.Inicializar();

            this.InitializeComponent();

            this.MainPage = Dependencia.Obtener<NavigationPage>(Dependencia.Obtener<IngresarPage>());
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
