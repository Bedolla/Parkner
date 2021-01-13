using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Reservas
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
        private IServicioReservas ServicioReservas { get; set; }

        [Inject]
        private IServicioClientes ServicioClientes { get; set; }

        [Inject]
        private IServicioEmpleados ServicioEmpleados { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private ReservaEditarModel Modelo { get; } = new();

        private Reserva Reserva { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async void Enviar()
        {
            try
            {
                this.Reserva.ClienteId = this.Modelo.ClienteSeleccionado;
                this.Reserva.EmpleadoInicializadorId = this.Modelo.EmpleadoIniSeleccionado;
                this.Reserva.EmpleadoFinalizadorId = this.Modelo.EmpleadoFinSeleccionado;
                this.Reserva.Disponible = this.Modelo.Disponible;
                this.Reserva.Inicio = this.Reserva.Inicio.Date;
                this.Reserva.Inicio = this.Reserva.Inicio.Add(this.Modelo.InicioT);
                this.Reserva.Fin = this.Modelo.Fin.Date;
                this.Reserva.Fin = this.Reserva.Fin.Add(this.Modelo.FinT);
                this.Reserva.Cobrado = this.Modelo.Cobrado;

                await this.ServicioReservas.EditarAsync(this.Reserva);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private async Task RecibirAsync()
        {
            try
            {
                this.Modelo.Clientes = (await this.ServicioClientes.ObtenerTodosAsync(new PaginacionPeticion {CantidadPorPagina = 1000, NumeroPagina = 1, OrdenarPor = "Apellido"})).Lista;
                this.Modelo.Empleados = (await this.ServicioEmpleados.ObtenerDeAsync(new PaginacionPeticion {Id = this.EstacionamientoState.Id, CantidadPorPagina = 1000, NumeroPagina = 1, OrdenarPor = "Apellido"})).Lista;

                this.Reserva = await this.ServicioReservas.ObtenerAsync(this.Id);
                this.Modelo.ClienteSeleccionado = this.Reserva.ClienteId;
                this.Modelo.EmpleadoIniSeleccionado = this.Reserva.EmpleadoInicializadorId;
                this.Modelo.EmpleadoFinSeleccionado = this.Reserva.EmpleadoFinalizadorId;
                this.Modelo.Disponible = this.Reserva.Disponible;
                this.Modelo.Inicio = this.Reserva.Inicio;
                this.Modelo.InicioT = this.Reserva.Inicio.TimeOfDay;
                this.Modelo.Fin = this.Reserva.Fin;
                this.Modelo.FinT = this.Reserva.Fin.TimeOfDay;
                this.Modelo.Cobrado = this.Reserva.Cobrado;
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private async void Borrar()
        {
            try
            {
                await this.ServicioReservas.BorrarAsync(this.Id);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private class ReservaEditarModel
        {
            [Required(ErrorMessage = "Cliente obligatorio")]
            public string ClienteSeleccionado { get; set; }

            public string EmpleadoIniSeleccionado { get; set; }

            public string EmpleadoFinSeleccionado { get; set; }

            public IList<Cliente> Clientes { get; set; }
            public bool Disponible { get; set; }

            [Required(ErrorMessage = "Inicio obligatorio")]
            public DateTime Inicio { get; set; }

            [Required(ErrorMessage = "Fin obligatorio")]
            public DateTime Fin { get; set; }

            public decimal? Cobrado { get; set; }
            public List<Empleado> Empleados { get; set; }
            public TimeSpan InicioT { get; set; }
            public TimeSpan FinT { get; set; }
        }
    }
}
