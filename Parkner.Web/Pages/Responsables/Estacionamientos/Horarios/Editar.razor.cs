using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Horarios
{
    [Authorize]
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioHorarios ServicioHorarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private HorarioEdtarModel Modelo { get; } = new HorarioEdtarModel();
        private Horario Horario { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Horario = await this.ServicioHorarios.ObtenerAsync(this.Id);
            this.Modelo.Dia = (Dias)this.Horario.DiaNumero;
            this.Modelo.Inicio = this.Horario.Inicio.TimeOfDay;
            this.Modelo.Fin = this.Horario.Fin.TimeOfDay;
        }

        private async void Enviar()
        {
            try
            {
                this.Horario.Dia = this.Modelo.Dia.ToString("G");
                this.Horario.DiaNumero = Convert.ToInt32(this.Modelo.Dia);
                this.Horario.Inicio = DateTime.Today.Add(this.Modelo.Inicio);
                this.Horario.Fin = DateTime.Today.Add(this.Modelo.Fin);

                await this.ServicioHorarios.EditarAsync(this.Horario);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.Horario.EstacionamientoId}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.Horario.EstacionamientoId}");

        private async void Borrar()
        {
            try
            {
                await this.ServicioHorarios.BorrarAsync(this.Id);

                this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.Horario.EstacionamientoId}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private class HorarioEdtarModel
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
