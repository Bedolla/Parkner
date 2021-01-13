using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class LeerBorrarViewModel : BaseViewModel
    {
        private string _buscado;
        private string _informacion;
        private Usuario _seleccionado;
        private ObservableCollection<Usuario> _usuarios;

        public LeerBorrarViewModel() { }

        public LeerBorrarViewModel
        (
            IServicioUsuarios servicioUsuarios,
            INavigation navegacion
        )
        {
            this.ServicioUsuarios = servicioUsuarios;
            this.Navegacion = navegacion;

            this.AgregarCommand = new Command(this.Agregar);
            this.BorrarCommand = new Command<Usuario>(this.Borrar);
            this.EditarCommand = new Command<Usuario>(this.Editar);
            this.BuscarCommand = new Command<string>(this.Buscar);
        }

        private INavigation Navegacion { get; }

        private IServicioUsuarios ServicioUsuarios { get; }

        public ObservableCollection<Usuario> Usuarios
        {
            get => this._usuarios;
            set
            {
                this._usuarios = value;
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

        public Usuario Seleccionado
        {
            get => this._seleccionado;
            set
            {
                this._seleccionado = value;
                if (value != null) this.Editar(value);
            }
        }

        public string Buscado
        {
            get => this._buscado;
            set
            {
                this._buscado = value;
                if (value.EsNulo()) this.Buscar(value);
                this.OnPropertyChanged();
            }
        }

        public ICommand AgregarCommand { get; set; }
        public ICommand BorrarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand BuscarCommand { get; set; }

        public async void ListarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = await this.ServicioUsuarios.ObtenerTodosAsync();

                if ((usuarios != null) && (usuarios.Count > 0))
                {
                    this.Usuarios = new ObservableCollection<Usuario>(usuarios
                                                                      .OrderBy(p => p.Apellido)
                                                                      .ThenBy(p => p.Nombre)
                                                                      .ThenBy(p => p.Correo)
                                                                      .ToList());

                    this.Informacion = $"{usuarios.Count} {(usuarios.Count.Equals(1) ? "usuario encontrado" : "usuarios encontrados")}";
                }
                else
                {
                    this.Usuarios = new ObservableCollection<Usuario>();
                    this.Informacion = "No hay usuarios, agrégue uno";
                }
            }
            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private async void Agregar() => await this.Navegacion.PushAsync(Dependencia.Obtener<CrearEditarPage>());

        private async void Editar(Usuario usuario) => await this.Navegacion.PushAsync(Dependencia.Obtener<CrearEditarPage>(usuario));

        private async void Borrar(Usuario usuario)
        {
            if (!await Application.Current.MainPage.DisplayAlert("Borrar", $"¿Realmente desea borrar a {usuario.Nombre}?", "Sí", "No")) return;

            try
            {
                await this.ServicioUsuarios.BorrarAsync(usuario.Id);

                this.ListarUsuarios();
            }
            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private async void Buscar(string consulta)
        {
            if (String.IsNullOrWhiteSpace(consulta))
            {
                this.ListarUsuarios();
            }
            else
            {
                consulta = consulta.RemoverDiacriticos().ToLower();

                this.Usuarios = new ObservableCollection<Usuario>
                (
                    (await this.ServicioUsuarios.ObtenerTodosAsync())
                    .Where
                    (u =>
                        u.Nombre.RemoverDiacriticos().ToLower().Contains(consulta) ||
                        u.Apellido.RemoverDiacriticos().ToLower().Contains(consulta) ||
                        u.Correo.RemoverDiacriticos().ToLower().Contains(consulta)
                    )
                    .OrderBy(u => u.Apellido)
                    .ThenBy(u => u.Nombre)
                    .ThenBy(u => u.Correo)
                    .ToList()
                );

                this.Informacion = $"{this.Usuarios.Count} {(this.Usuarios.Count.Equals(1) ? "usuario encontrado" : "usuarios encontrados")}";
            }
        }
    }
}
