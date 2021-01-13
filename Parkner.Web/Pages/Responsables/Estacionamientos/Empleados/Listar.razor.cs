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

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Empleados
{
    [Authorize]
    public partial class Listar
    {
        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private IServicioEmpleados ServicioEmpleados { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private ILogger<Responsables.Listar> Registro { get; set; }

        private List<Empleado> Empleados { get; set; }
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
                this.PaginacionPeticion.Id = this.EstacionamientoState.Id;
       
                ListaPaginada<Empleado> empleadosPaginados = await this.ServicioEmpleados.ObtenerDeAsync(this.PaginacionPeticion);
                this.Empleados = empleadosPaginados.Lista;
                this.PaginacionMetaData = empleadosPaginados.MetaData;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al obtener empleados paginados /responsables/estacionamientos/empleados: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Crear()
        {
            this.Navegacion.NavigateTo("/responsables/estacionamientos/empleados/crear");
        }

        private void Agregar()
        {
            this.Navegacion.NavigateTo("/responsables/estacionamientos/empleados/agregar");
        }
    }
}
