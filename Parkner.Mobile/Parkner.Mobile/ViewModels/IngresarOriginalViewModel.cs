using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class IngresarOriginalViewModel : BaseViewModel
    {
        public IngresarOriginalViewModel(IServicioSesion servicioSesion)
        {
            this.ServicioSesion = servicioSesion;

            this.IngresarCommand = new Command(this.Ingresar);
            this.RegistrarCommand = new Command(this.Registrar);
        }

        private IServicioSesion ServicioSesion { get; set; }

        public Cliente Cliente { get; set; } = new Cliente();
        public IngresarModel Modelo { get; set; } = new IngresarModel();

        public ICommand IngresarCommand { get; set; }

        public ICommand RegistrarCommand { get; set; }

        public string Informacion { get; set; }

        private async void Registrar() => await Dependencia.Navegacion.PushAsync(new RegistrarOriginalPage());

        private async void Ingresar()
        {
            try
            {
                if (this.Cliente.Correo.EsNulo() || this.Cliente.Clave.EsNulo()) return;

                this.Cliente = await this.ServicioSesion.IngresarCliente(this.Cliente);

                Application.Current.Properties[Propiedades.Autenticado] = true;
                Application.Current.Properties[Propiedades.Id] = this.Cliente.Id;
                Application.Current.Properties[Propiedades.Nombre] = this.Cliente.Nombre;
                Application.Current.Properties[Propiedades.Apellido] = this.Cliente.Apellido;
                Application.Current.Properties[Propiedades.Correo] = this.Cliente.Correo;
                Application.Current.Properties[Propiedades.Foto] = this.Cliente.Foto;
                Application.Current.Properties[Propiedades.Token] = this.Cliente.Token;

                Dependencia.Inicio = new InicioPage();
            }
            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        public class IngresarModel
        {
            public string Correo { get; set; }
            public string Clave { get; set; }
        }
    }
}
