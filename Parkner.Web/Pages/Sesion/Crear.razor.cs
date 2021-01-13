using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Sesion
{
    public partial class Crear
    {
        [Inject]
        private NavigationManager AdministradorNavegacion { get; set; }

        [Inject]
        private IServicioSesion ServicioSesion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private ClienteCrearModel Modelo { get; } = new ClienteCrearModel();
        private Cliente Cliente { get; } = new Cliente();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        protected override async Task OnAfterRenderAsync(bool firstRender) => await this.JsRuntime.InvokeVoidAsync("ColocarTemaIngresar");

        private async void Enviar()
        {
            string id = Guid.NewGuid().ToString();
            string fotoNueva = $"images/avatares/clientes/{id}.png";

            try
            {
                this.Cliente.Id = id;
                this.Cliente.Nombre = this.Modelo.Nombre;
                this.Cliente.Apellido = this.Modelo.Apellido;
                this.Cliente.Correo = this.Modelo.Correo;
                this.Cliente.Clave = this.Modelo.Clave.Encriptar();
                this.Cliente.Foto = fotoNueva;
                this.Cliente.Rol = Roles.Cliente;
                this.Cliente.Creacion = DateTime.Now;
                this.Cliente.Disponible = true;

                //await using (FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write))
                //{
                //    this.FotoMemoria.WriteTo(fotoArchivo);

                //    this.Cliente.Foto = fotoNueva;
                //}

                await this.FotoMemoria.ToArray().SubirFotoAsync($"{id}.png", Roles.Cliente);

                await this.ServicioSesion.CrearCliente(this.Cliente);

                if (await this.Mensajes.MostrarInformacion($"Se creó al cliente {this.Cliente.Nombre} {this.Cliente.Apellido} ({this.Cliente.Correo})")) this.AdministradorNavegacion.NavigateTo("/ingresar", true);
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotoNueva}")) File.Delete($"wwwroot/{fotoNueva}");
                this.Modelo.Foto = String.Empty;

                this.StateHasChanged();
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

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
