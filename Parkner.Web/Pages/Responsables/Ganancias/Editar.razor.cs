using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Ganancias
{
    [Authorize]
    public partial class Editar
    {
        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioGanancias ServicioGanancias { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private GananciaEditarModel Modelo { get; } = new GananciaEditarModel();

        private Ganancia Ganancia { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Ganancia = await this.ServicioGanancias.ObtenerAsync(this.Id);
            this.Modelo.Fecha = this.Ganancia.Fecha;
            this.Modelo.Cantidad = this.Ganancia.Cantidad;
        }

        private async void Enviar()
        {
            try
            {
                this.Ganancia.Fecha = Convert.ToDateTime(this.Modelo.Fecha);
                this.Ganancia.Cantidad = Convert.ToDecimal(this.Modelo.Cantidad);

                await this.ServicioGanancias.EditarAsync(this.Ganancia);

                this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");

        private class GananciaEditarModel
        {
            [Required(ErrorMessage = "Fecha obligatroria")]
            public DateTime? Fecha { get; set; }

            [Required(ErrorMessage = "Cantidad obligatroria")]
            public decimal? Cantidad { get; set; }
        }
    }
}
