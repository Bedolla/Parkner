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
    public partial class Crear
    {
        public Crear() => this.Modelo.Fecha = DateTime.Now;

        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioGanancias ServicioGanancias { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private GananciaCrearModel Modelo { get; } = new GananciaCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            try
            {
                await this.ServicioGanancias.CrearAsync(new Ganancia
                {
                    Id = Guid.NewGuid().ToString(),
                    Fecha = this.Modelo.Fecha,
                    Cantidad = this.Modelo.Cantidad,
                    ResponsableId = this.ResponsableState.Id
                });

                this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");

        private class GananciaCrearModel
        {
            [Required(ErrorMessage = "Cantidad obligatoria")]
            public decimal Cantidad { get; set; }

            [Required(ErrorMessage = "Fecha obligatoria")]
            public DateTime Fecha { get; set; }
        }
    }
}
