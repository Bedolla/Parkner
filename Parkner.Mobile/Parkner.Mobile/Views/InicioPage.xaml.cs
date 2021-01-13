using Parkner.Core.Constants;
using Parkner.Mobile.Helpers;
using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPage : MasterDetailPage
    {
        public InicioPage()
        {
            this.InitializeComponent();

            this.InicioViewModel = Dependencia.Obtener<InicioViewModel>();

            this.BindingContext = this.InicioViewModel;

            this.Detail = Application.Current.Properties[Propiedades.Rol].ToString() switch
            {
                Roles.Responsable => new NavigationPage(new ResponsablesEstacionamientosListarPage()) { BarTextColor = Device.RuntimePlatform == Device.Android ? Color.White : Color.FromHex("333942") },
                Roles.Empleado => new NavigationPage(new EmpleadoRevisarPage()) { BarTextColor = Device.RuntimePlatform == Device.Android ? Color.White : Color.FromHex("333942") },
                _ => new NavigationPage(new ClienteEstacionamientosListarPage()) { BarTextColor = Device.RuntimePlatform == Device.Android ? Color.White : Color.FromHex("333942") }
            };

            this.IsPresented = false;
        }

        private InicioViewModel InicioViewModel { get; }

        private void ItemSeleccionado(object transmisor, SelectedItemChangedEventArgs argumentos)
        {
            if (argumentos.SelectedItem is MenuOpcion itemSeleccionado)
            {
                Page pagina = (Page)Activator.CreateInstance(itemSeleccionado.Pagina);
                pagina.Title = itemSeleccionado.Titulo;
                this.Detail = new NavigationPage(pagina) {BarTextColor = Device.RuntimePlatform == Device.Android ? Color.White : Color.Black};
                this.IsPresented = false;
                this.ListViewVistas.SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            this.InicioViewModel.ActualizarPerfil();

            base.OnAppearing();
        }
    }

    public class MenuOpcion
    {
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public Type Pagina { get; set; }
    }
}
