using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Sesion
{
    [Authorize]
    public partial class Perfil
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private UsuarioState UsuarioState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioUsuarios ServicioUsuarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private PerfilModel Modelo { get; } = new PerfilModel();
        private Usuario Usuario { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Usuario = await this.ServicioUsuarios.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Usuario.Nombre;
            this.Modelo.Apellido = this.Usuario.Apellido;
            this.Modelo.Correo = this.Usuario.Correo;
        }

        private async void Enviar()
        {
            string fotoNueva = $"images/avatares/usuarios/{this.Usuario.Id}.png";

            try
            {
                //string fotoVieja = this.Usuario.Foto;

                if (this.FotoMemoria != null)
                {
                    //await using FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write);
                    //this.FotoMemoria.WriteTo(fotoArchivo);

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Usuario.Id}.png", this.Usuario.Rol);
                    this.Usuario.Foto = fotoNueva;
                }

                this.Usuario.Nombre = this.Modelo.Nombre;
                this.Usuario.Apellido = this.Modelo.Apellido;
                this.Usuario.Correo = this.Modelo.Correo;
                this.Usuario.Clave = ((this.Modelo.Clave != null) && (this.Modelo.ConfirmacionClave != null)) ? this.Modelo.Clave.Encriptar() : this.Usuario.Clave;

                await this.ServicioUsuarios.EditarAsync(this.Usuario);

                this.UsuarioState.Nombre = this.Usuario.Nombre;
                this.UsuarioState.Apellido = this.Usuario.Apellido;
                this.UsuarioState.Correo = this.Usuario.Correo;
                this.UsuarioState.Clave = this.Usuario.Clave;
                this.UsuarioState.Foto = this.Usuario.Foto;
                this.UsuarioState.Rol = this.Usuario.Rol;

                this.UsuarioState.CambioUsuario();

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");
                //if (this.FotoMemoria != null) await fotoVieja.BorrarFotoAsync();

                this.Navegacion.NavigateTo("/");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //await fotoNueva.BorrarFotoAsync();
                //if (File.Exists($"wwwroot/{fotoNueva}")) File.Delete($"wwwroot/{fotoNueva}");

                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/");

        private class PerfilModel
        {
            [Required(ErrorMessage = "Nombre(s) obligatorio(s)")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Apellido(s) obligatorio(s)")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "Correo obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
            public string Correo { get; set; }

            public string Clave { get; set; }

            [CompareProperty("Clave", ErrorMessage = "Debe ser igual a la contraseña")]
            public string ConfirmacionClave { get; set; }

            public string Foto { get; set; }
        }
    }
}
