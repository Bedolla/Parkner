using Parkner.Core.Constants;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class SalirViewModel
    {
        public SalirViewModel()
        {
            this.Salir();
        }

        private async void Salir()
        {
            Application.Current.Properties[Propiedades.Autenticado] = false;

            await Dependencia.Navegacion.PopAsync();

            Dependencia.Inicio = new NavigationPage(new IngresarPage());
        }
    }
}
