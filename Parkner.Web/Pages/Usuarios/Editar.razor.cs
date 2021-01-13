using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Usuarios
{
    [Authorize]
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioUsuarios ServicioUsuarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private UsuarioEditarModel Modelo { get; } = new UsuarioEditarModel();
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

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Usuario.Id}.png", "Usuario");
                    this.Usuario.Foto = fotoNueva;
                }

                this.Usuario.Nombre = this.Modelo.Nombre;
                this.Usuario.Apellido = this.Modelo.Apellido;
                this.Usuario.Correo = this.Modelo.Correo;
                this.Usuario.Clave = ((this.Modelo.Clave != null) && (this.Modelo.ConfirmacionClave != null)) ? this.Modelo.Clave.Encriptar() : this.Usuario.Clave;

                await this.ServicioUsuarios.EditarAsync(this.Usuario);

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");

                this.Navegacion.NavigateTo("/usuarios");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotoNueva}")) File.Delete($"wwwroot/{fotoNueva}");

                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/usuarios");

        private class UsuarioEditarModel
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
