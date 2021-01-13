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

namespace Parkner.Web.Pages.Responsables.Estacionamientos
{
    [Authorize]
    public partial class Listar
    {
        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private IServicioEstacionamientos ServicioEstacionamientos { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private ILogger<Listar> Registro { get; set; }

        private List<Estacionamiento> Estacionamientos { get; set; }
        private List<OrdenarOpciones> Opciones { get; set; }
        private PaginacionPeticion PaginacionPeticion { get; } = new PaginacionPeticion();
        private PaginacionMetaData PaginacionMetaData { get; set; } = new PaginacionMetaData();

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            this.PaginacionPeticion.OrdenarPor = "Nombre";

            this.Opciones = new List<OrdenarOpciones>
            {
                new OrdenarOpciones {Valor = "Nombre", Texto = "Nombre(s) ↑"},
                new OrdenarOpciones {Valor = "Nombre desc", Texto = "Nombre(s) ↓"},
                new OrdenarOpciones {Valor = "Tipo", Texto = "Tipo ↑"},
                new OrdenarOpciones {Valor = "Tipo desc", Texto = "Tipo ↓"},
                new OrdenarOpciones {Valor = "Concurrido", Texto = "Concurrido ↑"},
                new OrdenarOpciones {Valor = "Concurrido desc", Texto = "Concurrido ↓"},
                new OrdenarOpciones {Valor = "Calificacion", Texto = "Calificacion ↑"},
                new OrdenarOpciones {Valor = "Calificacion desc", Texto = "Calificacion ↓"}
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
                this.PaginacionPeticion.Id = this.ResponsableState.Id;

                ListaPaginada<Estacionamiento> estacionamientosPaginados = await this.ServicioEstacionamientos.ObtenerDeAsync(this.PaginacionPeticion);
                this.Estacionamientos = estacionamientosPaginados.Lista;
                this.PaginacionMetaData = estacionamientosPaginados.MetaData;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al obtener estacionamientos paginados /responsables/ver: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Crear() => this.Navegacion.NavigateTo("/responsables/estacionamientos/crear");
    }
}
