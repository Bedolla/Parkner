using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Constants;
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
    public partial class Crear
    {
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
        private EmpleadoCrearModel Modelo { get; } = new EmpleadoCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            string id = Guid.NewGuid().ToString();
            string fotoRuta = $"images/avatares/empleados/{id}.png";

            try
            {
                //await using (FileStream fotoArchivo = new FileStream($"wwwroot/{fotoRuta}", FileMode.Create, FileAccess.Write))
                //{
                //    this.FotoMemoria.WriteTo(fotoArchivo);
                //}

                await this.FotoMemoria.ToArray().SubirFotoAsync($"{id}.png", Roles.Empleado);

                await this.ServicioEmpleados.CrearAsync(new Empleado
                {
                    Id = id,
                    Nombre = this.Modelo.Nombre,
                    Apellido = this.Modelo.Apellido,
                    Correo = this.Modelo.Correo,
                    Clave = this.Modelo.Clave.Encriptar(),
                    Rol = this.EstacionamientoState.Id,
                    Foto = fotoRuta,
                    Creacion = DateTime.Now,
                    Disponible = true
                });

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
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

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class EmpleadoCrearModel
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
