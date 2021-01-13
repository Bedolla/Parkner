using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.Shared.Tablas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables
{
    [Authorize]
    public partial class Listar
    {
        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private IServicioResponsables ServicioResponsables { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private ILogger<Listar> Registro { get; set; }

        private List<Responsable> Responsables { get; set; }
        private List<OrdenarOpciones> Opciones { get; set; }
        private PaginacionPeticion PaginacionPeticion { get; } = new PaginacionPeticion();
        private PaginacionMetaData PaginacionMetaData { get; set; } = new PaginacionMetaData();

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            this.PaginacionPeticion.OrdenarPor = "Apellido";

            this.Opciones = new List<OrdenarOpciones>
            {
                new OrdenarOpciones {Valor = "Nombre", Texto = "Nombre(s) ↑"},
                new OrdenarOpciones {Valor = "Nombre desc", Texto = "Nombre(s) ↓"},
                new OrdenarOpciones {Valor = "Apellido", Texto = "Apellidos(s) ↑"},
                new OrdenarOpciones {Valor = "Apellido desc", Texto = "Apellidos(s) ↓"},
                new OrdenarOpciones {Valor = "Correo", Texto = "Correo ↑"},
                new OrdenarOpciones {Valor = "Correo desc", Texto = "Correo ↓"}
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
                ListaPaginada<Responsable> responsablePaginado = await this.ServicioResponsables.ObtenerTodoAsync(this.PaginacionPeticion);
                this.Responsables = responsablePaginado.Lista;
                this.PaginacionMetaData = responsablePaginado.MetaData;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al obtener responsables paginados /responsables: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Crear() => this.Navegacion.NavigateTo("/responsables/crear");
    }
}
