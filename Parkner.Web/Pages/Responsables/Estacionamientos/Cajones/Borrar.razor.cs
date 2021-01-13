using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Cajones
{
    [Authorize]
    public partial class Borrar
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

        private CajonBorrarModel Modelo { get; } = new CajonBorrarModel();

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

        private async void Enviar(EditContext obj)
        {
            try
            {
                await this.ServicioCajones.BorrarAsync(this.Id);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class CajonBorrarModel
        {
            public string Nombre { get; set; }

            public bool Disponible { get; set; }
        }
    }
}
