using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClienteEstacionamientosListarPage : ContentPage
    {
        public ClienteEstacionamientosListarPage()
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ClienteEstacionamientosListarViewModel>();
        }
    }
}
