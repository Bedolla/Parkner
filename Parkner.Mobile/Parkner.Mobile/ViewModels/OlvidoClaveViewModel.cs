using Parkner.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class OlvidoClaveViewModel : LoginViewModel
    {
        public OlvidoClaveViewModel() => this.SolicitarCommand = new Command(this.Solicitar);

        public Command SolicitarCommand { get; set; }

        private async void Solicitar(object obj)
        {
            await Application.Current.MainPage.DisplayAlert("Aviso", "Correo enviado", "Entendido");
            await Dependencia.Navegacion.PopAsync();
        }
    }
}
