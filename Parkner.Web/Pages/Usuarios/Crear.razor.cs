using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Constants;
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
    public partial class Crear
    {
        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioUsuarios ServicioUsuarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private UsuarioCrearModel Modelo { get; } = new UsuarioCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            string id = Guid.NewGuid().ToString();
            string fotoRuta = $"images/avatares/usuarios/{id}.png";

            try
            {
                //await using (FileStream fotoArchivo = new FileStream($"wwwroot/{fotoRuta}", FileMode.Create, FileAccess.Write))
                //{
                //    this.FotoMemoria.WriteTo(fotoArchivo);
                //}

                await this.FotoMemoria.ToArray().SubirFotoAsync($"{id}.png", "Usuario");

                await this.ServicioUsuarios.CrearAsync(new Usuario
                {
                    Id = id,
                    Nombre = this.Modelo.Nombre,
                    Apellido = this.Modelo.Apellido,
                    Correo = this.Modelo.Correo,
                    Clave = this.Modelo.Clave.Encriptar(),
                    Rol = Roles.Empleado,
                    Foto = fotoRuta,
                    Creacion = DateTime.Now,
                    Disponible = true
                });

                this.Navegacion.NavigateTo("/usuarios");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotoRuta}")) File.Delete($"wwwroot/{fotoRuta}");
                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/usuarios");

        private class UsuarioCrearModel
        {
            [Required(ErrorMessage = "Nombre(s) obligatorio(s)")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Apellido(s) obligatorio(s)")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "Correo obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "Clave obligatoria")]
            public string Clave { get; set; }

            [Required(ErrorMessage = "Confirmación de contraseña obligatoria")]
            [CompareProperty("Clave", ErrorMessage = "Debe ser igual a la contraseña")]
            public string ConfirmacionClave { get; set; }

            [Required(ErrorMessage = "Foto obligatoria")]
            public string Foto { get; set; }
        }
    }
}
