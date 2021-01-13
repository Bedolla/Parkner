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
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

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

        private CajonEditarModel Modelo { get; } = new CajonEditarModel();

        private Cajon Cajon { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Cajon = await this.ServicioCajones.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Cajon.Nombre;
            this.Modelo.Disponible = this.Cajon.Disponible;
        }

        private async void Enviar()
        {
            try
            {
                this.Cajon.Nombre = this.Modelo.Nombre;
                this.Cajon.Disponible = this.Modelo.Disponible;

                await this.ServicioCajones.EditarAsync(this.Cajon);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class CajonEditarModel
        {
            [Required(ErrorMessage = "Nombre obligatorio")]
            public string Nombre { get; set; }

            public bool Disponible { get; set; }
        }
    }
}
