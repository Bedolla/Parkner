using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Horarios
{
    [Authorize]
    public partial class Crear
    {
        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioHorarios ServicioHorarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private HorarioCrearModel Modelo { get; } = new HorarioCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            try
            {
                await this.ServicioHorarios.CrearAsync(new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = this.Modelo.Dia.ToString("G"),
                    DiaNumero = Convert.ToInt32(this.Modelo.Dia),
                    Inicio = DateTime.Today.Add(this.Modelo.Inicio),
                    Fin = DateTime.Today.Add(this.Modelo.Fin),
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

        private class HorarioCrearModel
        {
            [Required(ErrorMessage = "Día obligatorio")]
            public Dias Dia { get; set; }

            [Required(ErrorMessage = "Hora de inicio obligatoria")]
            public TimeSpan Inicio { get; set; }

            [Required(ErrorMessage = "Hora de fin obligatoria")]
            public TimeSpan Fin { get; set; }
        }
    }
}
