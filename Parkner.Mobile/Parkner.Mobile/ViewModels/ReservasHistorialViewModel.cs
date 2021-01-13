using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace Parkner.Mobile.ViewModels
{
    public class ReservasHistorialViewModel : BaseViewModel
    {
        private DateTime _fin;
        private DateTime _inicio;
        private ObservableCollection<Reserva> _reservas;
        private ICommand _tapCommand;

        public ReservasHistorialViewModel
        (
            IServicioReservas servicioReservas
        )
        {
            this.ServicioReservas = servicioReservas;

            this.Inicio = DateTime.Now;
            this.Fin = DateTime.Now.AddMinutes(30);
        }

        public ReservasHistorialViewModel() { }

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

        public ObservableCollection<Reserva> Reservas
        {
            get => this._reservas ??= new ObservableCollection<Reserva>();
            set
            {
                this._reservas = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand TapCommand
        {
            get => this._tapCommand ??= new Command<ItemTappedEventArgs>(this.ReservaSeleccionada);
            set => this._tapCommand = value;
        }

        private void ReservaSeleccionada(ItemTappedEventArgs argumentos)
        {
            if (argumentos.ItemData is Reserva reserva) Dependencia.Navegacion.PushAsync(Dependencia.Obtener<ReservasDetallesPage>(reserva.Id));
        }

        public override void Cablear()
        {
            this.Obtener();
        }

        private async void Obtener()
        {
            if (this.ServicioReservas is null) return;
            if (!Application.Current.Properties.ContainsKey(Propiedades.Id)) return;
            if (Application.Current.Properties.ContainsKey(Propiedades.Id).ToString().EsNulo()) return;
            if (await this.ServicioReservas.ObtenerDeAsync(Application.Current.Properties[Propiedades.Id].ToString()) is null) return;
            ListaPaginada<Reserva> reservasPaginadas = await this.ServicioReservas.ObtenerDeAsync(Application.Current.Properties[Propiedades.Id].ToString());
            if (reservasPaginadas?.Lista is null) return;

            this.Reservas = new ObservableCollection<Reserva>
            (
                (await this.ServicioReservas.ObtenerDeAsync(Application.Current.Properties[Propiedades.Id].ToString())).Lista.Where(r => r.EmpleadoFinalizador is not null && (r.Inicio > DateTime.Now.Subtract(TimeSpan.FromDays(31)))).ToList()
            );
        }
    }
}
