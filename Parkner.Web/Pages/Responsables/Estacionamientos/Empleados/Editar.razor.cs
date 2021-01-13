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

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Empleados
{
    [Authorize]
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioEmpleados ServicioEmpleados { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private EmpleadoEditarModel Modelo { get; } = new EmpleadoEditarModel();

        private Empleado Empleado { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Empleado = await this.ServicioEmpleados.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Empleado.Nombre;
            this.Modelo.Apellido = this.Empleado.Apellido;
            this.Modelo.Correo = this.Empleado.Correo;
        }

        private async void Enviar()
        {
            string fotoNueva = $"images/avatares/empleados/{this.Empleado.Id}.png";

            try
            {
                //string fotoVieja = this.Empleado.Foto;

                if (this.FotoMemoria != null)
                {
                    //await using FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write);
                    //this.FotoMemoria.WriteTo(fotoArchivo);

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Empleado.Id}.png", this.Empleado.Rol);
                    this.Empleado.Foto = fotoNueva;
                }

                this.Empleado.Nombre = this.Modelo.Nombre;
                this.Empleado.Apellido = this.Modelo.Apellido;
                this.Empleado.Correo = this.Modelo.Correo;
                this.Empleado.Clave = ((this.Modelo.Clave != null) && (this.Modelo.ConfirmacionClave != null)) ? this.Modelo.Clave.Encriptar() : this.Empleado.Clave;

                await this.ServicioEmpleados.EditarAsync(this.Empleado);

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
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

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class EmpleadoEditarModel
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
