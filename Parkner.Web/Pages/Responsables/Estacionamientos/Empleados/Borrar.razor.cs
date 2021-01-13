using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Empleados
{
    [Authorize]
    public partial class Borrar
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

        private EmpleadoBorrarModel Modelo { get; } = new EmpleadoBorrarModel();

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
            this.Modelo.Foto = this.Empleado.Foto;
        }

        private async void Enviar(EditContext obj)
        {
            try
            {
                await this.ServicioEmpleados.BorrarAsync(this.Id);
                await this.Empleado.Foto.BorrarFotoAsync();

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class EmpleadoBorrarModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public string Foto { get; set; }
        }
    }
}
