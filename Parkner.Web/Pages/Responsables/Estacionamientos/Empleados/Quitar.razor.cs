using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Empleados
{
    [Authorize]
    public partial class Quitar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioEmpleados ServicioEmpleados { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.EnviarAsync();
        }

        private async Task EnviarAsync()
        {
            try
            {
                await this.ServicioEmpleados.QuitarAsync(new Empleado
                {
                    Id = this.Id,
                    Rol = this.EstacionamientoState.Id
                });

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }
    }
}
