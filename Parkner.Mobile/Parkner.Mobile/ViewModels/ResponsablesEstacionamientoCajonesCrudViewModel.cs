using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Syncfusion.Data.Extensions;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class ResponsablesEstacionamientoCajonesCrudViewModel : BaseViewModel
    {
        private int _cajonesMinimos;
        private int _cantidadDeCajones;
        private ICommand _guardarCommand;

        public ResponsablesEstacionamientoCajonesCrudViewModel
        (
            IServicioEstacionamientos servicioEstacionamientos,
            IServicioCajones servicioCajones,
            string estacionamientoId
        )
        {
            this.ServicioEstacionamientos = servicioEstacionamientos;
            this.ServicioCajones = servicioCajones;
            this.EstacionamientoId = estacionamientoId;

            this.Recibir();
        }

        public ResponsablesEstacionamientoCajonesCrudViewModel() { }

        private IServicioEstacionamientos ServicioEstacionamientos { get; }
        private IServicioCajones ServicioCajones { get; }

        private string EstacionamientoId { get; }

        private Estacionamiento Estacionamiento { get; set; }

        public ICommand GuardarCommand
        {
            get => this._guardarCommand ??= new Command(this.Enviar);
            set => this._guardarCommand = value;
        }

        public int CantidadDeCajones
        {
            get => this._cantidadDeCajones;
            set
            {
                this._cantidadDeCajones = value;
                this.OnPropertyChanged();
            }
        }

        public int CajonesMinimos
        {
            get => this._cajonesMinimos;
            set
            {
                this._cajonesMinimos = value;
                this.OnPropertyChanged();
            }
        }

        private async void Recibir()
        {
            this.Estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.EstacionamientoId);
            this.CantidadDeCajones = this.Estacionamiento.Cajones.Count;

            this.CajonesMinimos = this.Estacionamiento.Reservas.Count(r => (r.EmpleadoInicializadorId is not null) && r.EmpleadoFinalizadorId is null);
            if (this.CajonesMinimos < 1) this.CajonesMinimos = 1;
        }

        private async void Enviar()
        {
            try
            {
                this.Estacionamiento.Cajones.ForEach(async c => await this.ServicioCajones.BorrarAsync(c.Id));

                for (int i = 0; i < this.CantidadDeCajones; i++)
                {
                    await this.ServicioCajones.CrearAsync(new Cajon
                    {
                        Id = Guid.NewGuid().ToString(),
                        Nombre = Guid.NewGuid().ToString(),
                        EstacionamientoId = this.EstacionamientoId,
                        Disponible = true
                    });
                }

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }
    }
}
