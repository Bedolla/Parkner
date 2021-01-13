using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Cajones
{
    [Authorize]
    public partial class Crear
    {
        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioCajones ServicioCajones { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private CajonCrearModel Modelo { get; } = new CajonCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            try
            {
                await this.ServicioCajones.CrearAsync(new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = this.Modelo.Nombre,
                    Disponible = true,
                    EstacionamientoId = this.EstacionamientoState.Id
                });

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class CajonCrearModel
        {
            [Required(ErrorMessage = "Nombre obligatorio")]
            public string Nombre { get; set; }
        }
    }
}
