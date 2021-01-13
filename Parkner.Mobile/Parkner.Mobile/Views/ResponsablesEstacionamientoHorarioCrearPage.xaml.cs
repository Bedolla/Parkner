using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponsablesEstacionamientoHorarioCrearPage
    {
        public ResponsablesEstacionamientoHorarioCrearPage(string id)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ResponsablesEstacionamientoHorarioCrearViewModel>(id);
        }
    }
}
