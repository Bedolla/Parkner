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

namespace Parkner.Web.Pages.Clientes
{
    [Authorize]
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioClientes ServicioClientes { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private ClienteEditarModel Modelo { get; } = new ClienteEditarModel();
        private Cliente Cliente { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Cliente = await this.ServicioClientes.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Cliente.Nombre;
            this.Modelo.Apellido = this.Cliente.Apellido;
            this.Modelo.Correo = this.Cliente.Correo;
        }

        private async void Enviar()
        {
            string fotoNueva = $"images/avatares/clientes/{this.Cliente.Id}.png";

            try
            {
                //string fotoVieja = this.Cliente.Foto;

                if (this.FotoMemoria != null)
                {
                    //await using FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write);
                    //this.FotoMemoria.WriteTo(fotoArchivo);

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Cliente.Id}.png", this.Cliente.Rol);
                    this.Cliente.Foto = fotoNueva;
                }

                this.Cliente.Nombre = this.Modelo.Nombre;
                this.Cliente.Apellido = this.Modelo.Apellido;
                this.Cliente.Correo = this.Modelo.Correo;
                this.Cliente.Clave = ((this.Modelo.Clave != null) && (this.Modelo.ConfirmacionClave != null)) ? this.Modelo.Clave.Encriptar() : this.Cliente.Clave;

                await this.ServicioClientes.EditarAsync(this.Cliente);

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");

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

        private class ClienteEditarModel
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
            [Compare("Clave", ErrorMessage = "Debe ser igual a la contraseña")]
            public string ConfirmacionClave { get; set; }

            public string Foto { get; set; }
        }
    }
}
