using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponsablesEstacionamientoEditarPage
    {
        public ResponsablesEstacionamientoEditarPage(string estacionamientoId)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ResponsablesEstacionamientoEditarViewModel>(estacionamientoId);
        }
    }
}
