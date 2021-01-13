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
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Reservas
{
    [Authorize]
    public partial class Crear
    {
        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioReservas ServicioReservas { get; set; }

        [Inject]
        private IServicioClientes ServicioClientes { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private ReservaCrearModel Modelo { get; } = new ReservaCrearModel();

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async void Enviar()
        {
            try
            {
                this.Modelo.Inicio = this.Modelo.Inicio.Date;
                this.Modelo.Inicio = this.Modelo.Inicio.Add(this.Modelo.InicioT);

                this.Modelo.Fin = this.Modelo.Fin.Date;
                this.Modelo.Fin = this.Modelo.Fin.Add(this.Modelo.FinT);

                await this.ServicioReservas.CrearAsync(new Reserva
                {
                    Id = Guid.NewGuid().ToString(),
                    ClienteId = this.Modelo.ClienteSeleccionado,
                    EstacionamientoId = this.EstacionamientoState.Id,
                    Inicio = this.Modelo.Inicio,
                    Fin = this.Modelo.Fin,
                    Tolerancia = 15,
                    Disponible = true
                });

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
                ListaPaginada<Cliente> clientesPaginados = await this.ServicioClientes.ObtenerTodosAsync(new PaginacionPeticion {CantidadPorPagina = 1000, NumeroPagina = 1, OrdenarPor = "Apellido"});
                List<Cliente> clientes = clientesPaginados.Lista;
                if (clientes is not null && clientes.Any()) this.Modelo.ClienteSeleccionado = clientes.First().Id;

                this.Modelo.Clientes = clientes;
                this.Modelo.Inicio = DateTime.Now.AddMinutes(30);
                this.Modelo.Fin = DateTime.Now.AddMinutes(60);
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class ReservaCrearModel
        {
            public IList<Cliente> Clientes { get; set; }

            [Required(ErrorMessage = "Cliente obligatorio")]
            public string ClienteSeleccionado { get; set; }

            [Required(ErrorMessage = "Fecha de inicio obligatoria")]
            public DateTime Inicio { get; set; }

            [Required(ErrorMessage = "Hora de inicio obligatoria")]
            public TimeSpan InicioT { get; set; }

            [Required(ErrorMessage = "Fecha de fin obligatoria")]
            public DateTime Fin { get; set; }

            [Required(ErrorMessage = "Hora de fin obligatoria")]
            public TimeSpan FinT { get; set; }
        }
    }
}
