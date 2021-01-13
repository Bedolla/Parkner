using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms;

namespace Parkner.Mobile.Views
{
    public partial class RegistrarOriginalPage : ContentPage
    {
        public RegistrarOriginalPage()
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<RegistrarOriginalViewModel>();
        }
    }
}
