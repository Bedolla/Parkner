using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeerBorrarPage : ContentPage
    {
        public LeerBorrarPage()
        {
            this.InitializeComponent();

            this.LeerBorrarViewModel = Dependencia.Obtener<LeerBorrarViewModel>(this.Navigation);

            this.BindingContext = this.LeerBorrarViewModel;
        }

        public LeerBorrarViewModel LeerBorrarViewModel { get; }

        protected override void OnAppearing() => this.LeerBorrarViewModel.ListarUsuarios();
    }
}
