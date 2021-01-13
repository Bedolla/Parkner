using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponsablesEstacionamientoHorarioEditarPage
    {
        public ResponsablesEstacionamientoHorarioEditarPage(string id)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ResponsablesEstacionamientoHorarioEditarViewModel>(id);
        }
    }
}
