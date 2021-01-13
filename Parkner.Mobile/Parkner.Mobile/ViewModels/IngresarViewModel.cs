using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class IngresarViewModel : BaseViewModel
    {
        private string _clave;
        private string _correo;

        public IngresarViewModel(IServicioSesion servicioSesion)
        {
            this.ServicioSesion = servicioSesion;

            this.IngresarCommand = new Command(this.Ingresar);
            this.RegistrarCommand = new Command(this.Registrar);
            this.OlvidoClaveCommand = new Command(this.OlvidoClave);
        }

        public IngresarViewModel() { }

        public string Correo
        {
            get => this._correo;

            set
            {
                if (this._correo == value) return;

                this._correo = value;
                this.OnPropertyChanged();
            }
        }

        public string Clave
        {
            get => this._clave;

            set
            {
                if (this._clave == value) return;

                this._clave = value;
                this.OnPropertyChanged();
            }
        }

        private IServicioSesion ServicioSesion { get; }
        public ICommand IngresarCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }
        public Command OlvidoClaveCommand { get; set; }

        private async void OlvidoClave(object etiquetaCliqueada)
        {
            Label etiqueta = (Label)etiquetaCliqueada;
            etiqueta.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            etiqueta.BackgroundColor = Color.Transparent;

            await Dependencia.Navegacion.PushAsync(new OlvidoClavePage());
        }

        private async void Registrar() => await Dependencia.Navegacion.PushAsync(new RegistrarPage());

        private async void Ingresar()
        {
            try
            {
                if (this.Correo.EsNulo() || this.Clave.EsNulo()) return;

                this.Ocupado = true;

                Usuario usuario = await this.ServicioSesion.Ingresar(new Sesion {Correo = this.Correo, Clave = this.Clave});

                Application.Current.Properties[Propiedades.Autenticado] = true;
                Application.Current.Properties[Propiedades.Id] = usuario.Id;
                Application.Current.Properties[Propiedades.Nombre] = usuario.Nombre;
                Application.Current.Properties[Propiedades.Apellido] = usuario.Apellido;
                Application.Current.Properties[Propiedades.Correo] = usuario.Correo;
                Application.Current.Properties[Propiedades.Foto] = usuario.Foto;
                Application.Current.Properties[Propiedades.Rol] = usuario.Rol;
                Application.Current.Properties[Propiedades.Token] = usuario.Token;

                this.Ocupado = false;

                await Dependencia.Navegacion.PopAsync();

                Dependencia.Inicio = new InicioPage();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
            finally
            {
                this.Ocupado = false;
            }
        }
    }
}
