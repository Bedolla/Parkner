using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class ReservasCrearViewModel : BaseViewModel
    {
        private DateTime _fin;
        private DateTime _inicio;

        public ReservasCrearViewModel
        (
            IServicioReservas servicioReservas
        )
        {
            this.ServicioReservas = servicioReservas;

            this.CrearCommand = new Command(this.Crear);

            this.Inicio = DateTime.Now;
            this.Fin = DateTime.Now.AddMinutes(30);
        }

        public ReservasCrearViewModel() { }

        private IServicioReservas ServicioReservas { get; }

        public DateTime Inicio
        {
            get => this._inicio;
            set
            {
                this._inicio = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime Fin
        {
            get => this._fin;
            set
            {
                this._fin = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand CrearCommand { get; set; }

        // ToDo: Si Hay Reservas Activas a esa hora y ya no hay cajones, decir que intente otro horario

        private void Crear()
        {
            Reserva reservas = new Reserva();
            reservas.Inicio = this.Inicio;
            reservas.Fin = this.Fin;
        }
    }
}
