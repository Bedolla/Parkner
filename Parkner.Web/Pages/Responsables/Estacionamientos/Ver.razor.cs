using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos
{
    [Authorize]
    public partial class Ver
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioEstacionamientos ServicioEstacionamientos { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private EstacionamientoVerModel Modelo { get; } = new EstacionamientoVerModel();

        private Estacionamiento Estacionamiento { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.EstacionamientoState.Id = this.Id;

            this.Estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Estacionamiento.Nombre;
            this.Modelo.Descripcion = this.Estacionamiento.Descripcion;
            this.Modelo.Tipo = this.Estacionamiento.Tipo;
            this.Modelo.Foto = this.Estacionamiento.Foto;
            this.Modelo.Creacion = this.Estacionamiento.Creacion;
            this.Modelo.Calificacion = this.Estacionamiento.Calificacion;
            this.Modelo.Costo = this.Estacionamiento.Costo;
            this.Modelo.Concurrido = this.Estacionamiento.Concurrido;
            this.Modelo.Numero = this.Estacionamiento.Direccion.Numero;
            this.Modelo.Calle = this.Estacionamiento.Direccion.Calle;
            this.Modelo.EntreCalles = this.Estacionamiento.Direccion.EntreCalles;
            this.Modelo.Colonia = this.Estacionamiento.Direccion.Colonia;
            this.Modelo.CodigoPostal = this.Estacionamiento.Direccion.CodigoPostal;
            this.Modelo.Municipio = this.Estacionamiento.Direccion.Municipio;
            this.Modelo.Horarios = this.Estacionamiento.Horarios.ToList();
            this.Modelo.CajonesTotales = this.Estacionamiento.Cajones.Count;
            this.Modelo.Cajones = this.Modelo.CajonesTotales - this.Estacionamiento.Reservas.Count(r => (r.EmpleadoInicializadorId is not null) && r.EmpleadoFinalizadorId is null);
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");

        private void CrearHorario()
        {
            this.Navegacion.NavigateTo("/responsables/estacionamientos/horarios/crear");
        }

        private class EstacionamientoVerModel
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string Tipo { get; set; }
            public DateTime Creacion { get; set; }
            public string Foto { get; set; }
            public decimal? Calificacion { get; set; }
            public decimal Costo { get; set; }
            public bool Concurrido { get; set; }
            public string Numero { get; set; }
            public string Calle { get; set; }
            public string EntreCalles { get; set; }
            public string Colonia { get; set; }
            public string CodigoPostal { get; set; }
            public string Municipio { get; set; }
            public List<Horario> Horarios { get; set; } = new List<Horario>();
            public int Cajones { get; set; }
            public int CajonesTotales { get; set; }
        }
    }
}
