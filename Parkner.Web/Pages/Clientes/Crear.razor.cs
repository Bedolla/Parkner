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

namespace Parkner.Web.Pages.Clientes
{
    [Authorize]
    public partial class Crear
    {
        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioClientes ServicioClientes { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private ClienteCrearModel Modelo { get; } = new ClienteCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            string id = Guid.NewGuid().ToString();
            string fotoNueva = $"images/avatares/clientes/{id}.png";

            try
            {
                //await using (FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write))
                //{
                //    this.FotoMemoria.WriteTo(fotoArchivo);
                //}

                await this.FotoMemoria.ToArray().SubirFotoAsync($"{id}.png", Roles.Cliente);
              
                await this.ServicioClientes.CrearAsync(new Cliente
                {
                    Id = id,
                    Nombre = this.Modelo.Nombre,
                    Apellido = this.Modelo.Apellido,
                    Correo = this.Modelo.Correo,
                    Clave = this.Modelo.Clave.Encriptar(),
                    Rol = Roles.Empleado,
                    Foto = fotoNueva,
                    Creacion = DateTime.Now,
                    Disponible = true
                });

                this.Navegacion.NavigateTo("/clientes");
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

        private void Cancelar() => this.Navegacion.NavigateTo("/clientes");

        private class ClienteCrearModel
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
