using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Empleados
{
    [Authorize]
    public partial class Agregar
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

        private EmpleadoAgregarModel Modelo { get; } = new EmpleadoAgregarModel();

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            try
            {
                List<Empleado> empleados = await this.ServicioEmpleados.ObtenerNoDeAsync(this.EstacionamientoState.Id);

                if (empleados is not null && empleados.Any()) this.Modelo.EmpleadoSeleccionado = empleados.First().Id;

                this.Modelo.Empleados = empleados;
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private async void Enviar()
        {
            try
            {
                await this.ServicioEmpleados.AgregarAlAsync(new Empleado
                {
                    Id = this.Modelo.EmpleadoSeleccionado,
                    Rol = this.EstacionamientoState.Id
                });

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class EmpleadoAgregarModel
        {
            [Required(ErrorMessage = "Empleado requerido")]
            public string EmpleadoSeleccionado { get; set; }

            public List<Empleado> Empleados { get; set; }
        }
    }
}
