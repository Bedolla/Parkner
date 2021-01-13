using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class ResponsablesEstacionamientoHorarioCrearViewModel : BaseViewModel
    {
        private ICommand _crearCommand;
        private string _dia;
        private TimeSpan _fin;
        private TimeSpan _inicio;

        public ResponsablesEstacionamientoHorarioCrearViewModel
        (
            IServicioHorarios servicioHorarios,
            string estacionamientoId
        )
        {
            this.ServicioHorarios = servicioHorarios;
            this.EstacionamientoId = estacionamientoId;

            this.Recibir();
        }

        public ResponsablesEstacionamientoHorarioCrearViewModel() { }

        private IServicioHorarios ServicioHorarios { get; }

        private string EstacionamientoId { get; }

        private Horario Horario { get; set; }

        public string Dia
        {
            get => this._dia;
            set
            {
                this._dia = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan Fin
        {
            get => this._fin;
            set
            {
                this._fin = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan Inicio
        {
            get => this._inicio;
            set
            {
                this._inicio = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand CrearCommand
        {
            get => this._crearCommand ??= new Command(this.Enviar);
            set => this._crearCommand = value;
        }

        private void Recibir()
        {
            this.Horario = new Horario();
            this.Dia = "Lunes";
            this.Inicio = new DateTime(2000, 1, 1, 7, 0, 0).TimeOfDay;
            this.Fin = new DateTime(2000, 1, 1, 19, 0, 0).TimeOfDay;
        }

        private async void Enviar()
        {
            try
            {
                await this.ServicioHorarios.CrearAsync(new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = this.Dia,
                    DiaNumero = Convert.ToInt32(Enum.Parse(typeof(Dias), this.Dia)),
                    Inicio = DateTime.Today.Add(this.Inicio),
                    Fin = DateTime.Today.Add(this.Fin),
                    EstacionamientoId = this.EstacionamientoId
                });

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }
    }
}
