using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.Shared.Tablas;
using Parkner.Web.States;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Cajones
{
    [Authorize]
    public partial class Listar
    {
        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private IServicioCajones ServicioCajones { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private ILogger<Reservas.Listar> Registro { get; set; }

        private List<Cajon> Cajones { get; set; }
        private List<OrdenarOpciones> Opciones { get; set; }
        private PaginacionPeticion PaginacionPeticion { get; } = new PaginacionPeticion();
        private PaginacionMetaData PaginacionMetaData { get; set; } = new PaginacionMetaData();

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            this.PaginacionPeticion.OrdenarPor = "Nombre";

            this.Opciones = new List<OrdenarOpciones>
            {
                new OrdenarOpciones {Valor = "Nombre", Texto = "Nombre ↑"},
                new OrdenarOpciones {Valor = "Nombre desc", Texto = "Nombre ↓"},
                new OrdenarOpciones {Valor = "Disponible", Texto = "Disponible ↑"},
                new OrdenarOpciones {Valor = "Disponible desc", Texto = "Disponible ↓"}
            };

            this.PaginacionPeticion.CantidadPorPagina = 5;
            await this.RecibirAsync();
        }

        private async Task PaginaSeleccionada(int pagina)
        {
            this.PaginacionPeticion.NumeroPagina = pagina;
            await this.RecibirAsync();
        }

        private async Task BusquedaCambio(string terminoBuscado)
        {
            this.PaginacionPeticion.NumeroPagina = 1;
            this.PaginacionPeticion.TerminoBuscado = terminoBuscado;
            await this.RecibirAsync();
        }

        private async Task OrdenCambio(string ordenarPor)
        {
            this.PaginacionPeticion.OrdenarPor = ordenarPor;
            await this.RecibirAsync();
        }

        private async Task CantidadCambio(int tamano)
        {
            this.PaginacionPeticion.CantidadPorPagina = tamano;
            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            try
            {
                this.PaginacionPeticion.Id = this.EstacionamientoState.Id;
                ListaPaginada<Cajon> cajonesPaginados = await this.ServicioCajones.ObtenerDeAsync(this.PaginacionPeticion);
                this.Cajones = cajonesPaginados.Lista;
                this.PaginacionMetaData = cajonesPaginados.MetaData;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al obtener cajones paginados /responsables/estacionamientos/ver: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Crear()
        {
            this.Navegacion.NavigateTo("/responsables/estacionamientos/cajones/crear");
        }
    }
}
