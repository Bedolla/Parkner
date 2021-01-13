using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class RegistrarOriginalViewModel : BaseViewModel
    {
        private Cliente _cliente;
        private string _informacion;

        public RegistrarOriginalViewModel
        (
            IServicioClientes servicioClientes
        )
        {
            this.ServicioClientes = servicioClientes;

            this.LimpiarCommand = new Command(this.Limpiar);
            this.GuardarCommand = new Command(this.Guardar);
        }

        public RegistrarOriginalViewModel() { }

        private IServicioClientes ServicioClientes { get; }
        public ICommand LimpiarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public Cliente Cliente
        {
            get => this._cliente ??= new Cliente();
            set
            {
                this._cliente = value;
                this.OnPropertyChanged();
            }
        }

        public string Informacion
        {
            get => this._informacion;
            set
            {
                this._informacion = value;
                this.OnPropertyChanged();
            }
        }

        private async void Guardar()
        {
            try
            {
                if
                (
                    this.Cliente.Nombre.EsNulo() ||
                    this.Cliente.Apellido.EsNulo() ||
                    this.Cliente.Correo.EsNulo() ||
                    this.Cliente.Clave.EsNulo() ||
                    this.Cliente.Foto.EsNulo()
                )
                {
                    this.Informacion = "Debe llenar todos los campos";
                    return;
                }

                Cliente clienteEncontrado = await this.ServicioClientes.ObtenerAsync(this.Cliente.Correo);
                if (clienteEncontrado is null || (clienteEncontrado.Id == this.Cliente.Id))
                {
                    this.Cliente.Id = Guid.NewGuid().ToString();
                    this.Cliente.Rol = Roles.Cliente;
                    this.Cliente.Creacion = DateTime.Now;
                    this.Cliente.Disponible = true;

                    await this.ServicioClientes.CrearAsync(this.Cliente);

                    await Application.Current.MainPage.DisplayAlert("Aviso", "Usuario editado", "Entendido");
                    await Dependencia.Navegacion.PopAsync();

                    Page rootPage = Dependencia.Navegacion.NavigationStack.FirstOrDefault();

                    if (rootPage == null) return;

                    //                   App.EstaAutenticado = true;

                    Dependencia.Navegacion.InsertPageBefore(new InicioPage(), Dependencia.Navegacion.NavigationStack.First());

                    await Dependencia.Navegacion.PopToRootAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Duplicado", "Ya hay un usuario con ese Correo.", "Entendido");
                }
            }
            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private void Limpiar()
        {
            this.Cliente = new Cliente
            {
                Id = this.Cliente?.Id.Valor(),
                Nombre = String.Empty,
                Apellido = String.Empty,
                Correo = String.Empty,
                Clave = String.Empty,
                Foto = String.Empty
            };

            this.Informacion = String.Empty;
        }
    }
}
