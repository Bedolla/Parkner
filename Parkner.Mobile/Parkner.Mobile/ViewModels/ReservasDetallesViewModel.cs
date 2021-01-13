using Parkner.Data.Entities;
using Parkner.Mobile.Models;
using Parkner.Mobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Parkner.Mobile.ViewModels
{
    internal class ReservasDetallesViewModel : BaseViewModel
    {
        private ICommand _cancelarCommand;
        private ObservableCollection<Pin> _pines;
        private Posicion _posision;
        private Reserva _reserva;

        public ReservasDetallesViewModel
        (
            IServicioReservas servicioReservas,
            string id
        )
        {
            this.ServicioReservas = servicioReservas;
            this.Id = id;
        }

        public ReservasDetallesViewModel() { }

        private IServicioReservas ServicioReservas { get; }

        public ICommand CancelarCommand
        {
            get => this._cancelarCommand ??= new Command(this.Cancelar);
            set => this._cancelarCommand = value;
        }

        private string Id { get; }

        public Reserva Reserva
        {
            get => this._reserva;
            set
            {
                this._reserva = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Pin> Pines
        {
            get => this._pines ??= new ObservableCollection<Pin>();
            set
            {
                this._pines = value;
                this.OnPropertyChanged();
            }
        }

        public Posicion Posision
        {
            get => this._posision ??= new Posicion();
            set
            {
                this._posision = value;
                this.OnPropertyChanged();
            }
        }

        public override void Cablear()
        {
            this.Obtener();
        }

        private async void Obtener()
        {
            if (this.ServicioReservas is null) return;
            if (this.Id is null) return;
            if (await this.ServicioReservas.ObtenerAsync(this.Id) is null) return;
            this.Reserva = await this.ServicioReservas.ObtenerAsync(this.Id);

            this.Posision = new Posicion(Convert.ToDouble(this.Reserva.Estacionamiento.Direccion.Latitud), Convert.ToDouble(this.Reserva.Estacionamiento.Direccion.Longitud));

            this.Pines.Clear();

            this.Pines.Add(new Pin
            {
                AutomationId = this.Reserva.Estacionamiento.Id,
                Label = this.Reserva.Estacionamiento.Nombre,
                Address = $"{this.Reserva.Estacionamiento.Direccion.Calle} {this.Reserva.Estacionamiento.Direccion.Numero}, {this.Reserva.Estacionamiento.Direccion.Colonia}",
                Position = new Position(this.Posision.Latitud, this.Posision.Longitud),
                Type = PinType.Place
            });
        }

        private async void Cancelar()
        {
            if (await Application.Current.MainPage.DisplayAlert("Cancelar", "¿Realmente desea cancelar la reserva?", "Sí", "No")) await this.ServicioReservas.BorrarAsync(this.Id);
        }
    }
}
