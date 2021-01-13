using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearEditarPage : ContentPage
    {
        public CrearEditarPage(Usuario usuario)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<CrearEditarViewModel>(this.Navigation, usuario);
        }
    }
}
