using Parkner.Core.Constants;
using Parkner.Core.Utilities;
using Parkner.Mobile.Helpers;
using Parkner.Mobile.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class InicioViewModel : BaseViewModel
    {
        private string _avatar;
        private string _correo;
        private ImageSource _iconoMenu;
        private string _nombre;

        public InicioViewModel()
        {
            this.Menu = Application.Current.Properties[Propiedades.Rol].ToString() switch
            {
                Roles.Responsable => new ObservableCollection<MenuOpcion>(new[]
                {
                    new MenuOpcion {Titulo = "Perfil", Pagina = typeof(PerfilPage), Icono = FontAwesomeIcons.UserEdit},
                    new MenuOpcion {Titulo = "Estacionamientos", Pagina = typeof(ResponsablesEstacionamientosListarPage), Icono = FontAwesomeIcons.Parking},
                    new MenuOpcion {Titulo = "Ganancias", Pagina = typeof(ResponsablesGananciasPage), Icono = FontAwesomeIcons.MoneyBill},
                    new MenuOpcion {Titulo = "Salir", Pagina = typeof(SalirPage), Icono = FontAwesomeIcons.SignOut}
                }),
                Roles.Empleado => new ObservableCollection<MenuOpcion>(new[]
                {
                    new MenuOpcion {Titulo = "Perfil", Pagina = typeof(PerfilPage), Icono = FontAwesomeIcons.UserEdit},
                    new MenuOpcion {Titulo = "Reservas", Pagina = typeof(EmpleadoRevisarPage), Icono = FontAwesomeIcons.MoneyBill},
                    new MenuOpcion {Titulo = "Salir", Pagina = typeof(SalirPage), Icono = FontAwesomeIcons.SignOut}
                }),
                _ => new ObservableCollection<MenuOpcion>(new[]
                {
                    new MenuOpcion {Titulo = "Perfil", Pagina = typeof(PerfilPage), Icono = FontAwesomeIcons.UserEdit},
                    new MenuOpcion {Titulo = "Estacionamientos", Pagina = typeof(ClienteEstacionamientosListarPage), Icono = FontAwesomeIcons.Parking},
                    new MenuOpcion {Titulo = "Reservas", Pagina = typeof(ReservasListarPage), Icono = FontAwesomeIcons.MoneyBill},
                    new MenuOpcion {Titulo = "Salir", Pagina = typeof(SalirPage), Icono = FontAwesomeIcons.SignOut}
                })
            };
        }

        public ObservableCollection<MenuOpcion> Menu { get; }

        public string Nombre
        {
            get => this._nombre;
            set
            {
                this._nombre = value;
                this.OnPropertyChanged();
            }
        }

        public string Correo
        {
            get => this._correo;
            set
            {
                this._correo = value;
                this.OnPropertyChanged();
            }
        }

        public string Avatar
        {
            get => this._avatar;
            set
            {
                this._avatar = value;
                this.OnPropertyChanged();
            }
        }

        public ImageSource IconoMenu
        {
            get => this._iconoMenu;
            set
            {
                this._iconoMenu = value;
                this.OnPropertyChanged();
            }
        }

        public void ActualizarPerfil()
        {
            this.Nombre = $"{Application.Current.Properties[Propiedades.Nombre]} {Application.Current.Properties[Propiedades.Apellido]}";
            this.Correo = Application.Current.Properties[Propiedades.Correo].ToString();
            this.Avatar = $"{Uris.Fotos}{Application.Current.Properties[Propiedades.Foto]}";

            this.IconoMenu = ImageSource.FromStream(() => Imagenes.Menu);
        }
    }
}
