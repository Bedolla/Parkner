using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Mobile.Models;
using Parkner.Mobile.Services;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Parkner.Mobile.ViewModels
{
    public class ClienteEstacionamientosVerViewModel : BaseViewModel
    {
        private string _cajones;
        private int _cantidadReservas;
        private string _direccion;
        private Estacionamiento _estacionamiento;
        private DateTime _fechaMinima;
        private DateTime _fin;
        private string _foto;
        private bool _habilitado;
        private ObservableCollection<Horario> _horarios;
        private DateTime _inicio;
        private bool _mostrarReservas;
        private ObservableCollection<Pin> _pines;
        private Posicion _posision;
        private Command _reservarCommand;
        private ObservableCollection<ReservaActiva> _reservas;
        private Location _ubicacion;

        public ClienteEstacionamientosVerViewModel
        (
            IServicioEstacionamientos servicioEstacionamientos,
            IServicioReservas servicioReservas,
            string id
        )
        {
            this.ServicioEstacionamientos = servicioEstacionamientos;
            this.ServicioReservas = servicioReservas;
            this.Id = id;

            this.Obtener();
        }

        public ClienteEstacionamientosVerViewModel() { }

        private IServicioEstacionamientos ServicioEstacionamientos { get; }

        private IServicioReservas ServicioReservas { get; }

        public ICommand ReservarCommand => this._reservarCommand ??= new Command(this.Reservar);

        private string Id { get; }

        public string Direccion
        {
            get => this._direccion;
            set
            {
                this._direccion = value;
                this.OnPropertyChanged();
            }
        }

        public string Foto
        {
            get => Uris.Fotos + this._foto;
            set
            {
                this._foto = value;
                this.OnPropertyChanged();
            }
        }

        public Estacionamiento Estacionamiento
        {
            get => this._estacionamiento;
            set
            {
                this._estacionamiento = value;
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

        public Location Ubicacion
        {
            get => this._ubicacion;
            set
            {
                this._ubicacion = value;
                this.OnPropertyChanged();
            }
        }

        public string Cajones
        {
            get => this._cajones;
            set
            {
                this._cajones = value;
                this.OnPropertyChanged();
            }
        }

        public int CantidadReservas
        {
            get => this._cantidadReservas;
            set
            {
                this._cantidadReservas = value;
                this.OnPropertyChanged();
            }
        }

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

        public DateTime FechaMinima
        {
            get => this._fechaMinima;
            set
            {
                this._fechaMinima = value;
                this.OnPropertyChanged();
            }
        }

        public bool Habilitado
        {
            get => this._habilitado;
            set
            {
                this._habilitado = value;
                this.OnPropertyChanged();
            }
        }

        public bool MostrarReservas
        {
            get => this._mostrarReservas;
            set
            {
                this._mostrarReservas = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<ReservaActiva> Reservas
        {
            get => this._reservas ??= new ObservableCollection<ReservaActiva>();
            set => this._reservas = value;
        }

        private int CajonesTotales { get; set; }

        private int CajonesDisponibles { get; set; }

        public ObservableCollection<Horario> Horarios
        {
            get => this._horarios ??= new ObservableCollection<Horario>();
            set
            {
                this._horarios = value;
                this.OnPropertyChanged();
            }
        }

        private async void Obtener()
        {
            this.FechaMinima = DateTime.Now;
            this.Inicio = DateTime.Now.AddMinutes(30);
            this.Fin = DateTime.Now.AddMinutes(60);

            this.Estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.Id);

            this.Reservas.Clear();

            this.Estacionamiento.Reservas
                .Where(r => r.Disponible)
                .OrderBy(r => r.Inicio)
                .ThenBy(r => r.Fin)
                .ForEach(r => this.Reservas.Add(new ReservaActiva
                {
                    Inicio = r.Inicio,
                    Fin = r.Fin,
                    Duracion = $"{(r.Fin - r.Inicio).TotalMinutes:N0} minutos"
                }));

            this.CantidadReservas = this.Estacionamiento.Reservas.Count(r => r.Disponible);
            this.MostrarReservas = this.CantidadReservas > 0;
            this.Horarios = new ObservableCollection<Horario>(this.Estacionamiento.Horarios.OrderBy(h => h.DiaNumero).ThenBy(h => h.Inicio).ThenBy(h => h.Fin).ToList());

            this.Foto = this.Estacionamiento.Foto;
            this.Direccion = $"{this.Estacionamiento.Direccion.Calle} {this.Estacionamiento.Direccion.Numero}, {this.Estacionamiento.Direccion.Colonia} {this.Estacionamiento.Direccion.CodigoPostal}, {this.Estacionamiento.Direccion.Municipio}";

            this.CajonesTotales = this.Estacionamiento.Cajones.Count;
            this.CajonesDisponibles = this.CajonesTotales - this.Estacionamiento.Reservas.Count(r => (r.EmpleadoInicializadorId is not null) && r.EmpleadoFinalizadorId is null);

            this.Habilitado = this.CajonesDisponibles > 0;

            this.Cajones = $"{this.CajonesDisponibles}/{this.CajonesTotales} cajones";
            this.Posision = new Posicion(Convert.ToDouble(this.Estacionamiento.Direccion.Latitud), Convert.ToDouble(this.Estacionamiento.Direccion.Longitud));

            Pin pin = new Pin
            {
                AutomationId = this.Estacionamiento.Id,
                Label = this.Estacionamiento.Nombre,
                Address = $"{this.Estacionamiento.Direccion.Calle} {this.Estacionamiento.Direccion.Numero}, {this.Estacionamiento.Direccion.Colonia}",
                Position = new Position(this.Posision.Latitud, this.Posision.Longitud),
                Type = PinType.Place
            };

            this.Pines.Add(pin);
        }

        private async void Reservar()
        {
            try
            {
                this.Ocupado = true;
                this.Habilitado = false;

                if (this.CajonesDisponibles < 1)
                {
                    Dependencia.Avisar("Ya no hay espacio en éste estacionamiento");
                    this.Obtener();
                    return;
                }

                if (this.Inicio > this.Fin)
                {
                    Dependencia.Avisar("No puede elegir una fecha final anterior a la de inicio");
                    return;
                }

                if
                (
                    (this.Inicio < DateTime.Now) ||
                    (this.Fin < DateTime.Now)
                )
                {
                    Dependencia.Avisar("No puede elegir tiempo pasado");
                    return;
                }

                List<Reserva> reservasExistentes = this.Estacionamiento.Reservas.Where(r => (r.Inicio < this.Fin) && (this.Inicio < r.Fin)).Select(r => r).ToList();
                if (reservasExistentes.Any() && (reservasExistentes.Count >= this.CajonesTotales))
                {
                    Dependencia.Avisar($"Hay {reservasExistentes.Count} {(reservasExistentes.Count == 1 ? "reserva" : "reservas")} en ese rango de tiempo");
                    return;
                }

                DateTime inicio = this.Inicio;
                DateTime fin = this.Fin;
                List<string> diasElegidos = new() {inicio.ToString("dddd", new CultureInfo("es-MX")).ToLower()};
                while (inicio.Date < fin.Date)
                {
                    inicio = inicio.AddDays(1);
                    diasElegidos.Add(inicio.ToString("dddd", new CultureInfo("es-MX")).ToLower());
                }

                diasElegidos = diasElegidos.Distinct().ToList();
                List<string> diasDisponibles = this.Estacionamiento.Horarios.Select(h => h.Dia.ToLower()).ToList();

                if
                (
                    this.Estacionamiento.Horarios.All
                    (
                        h =>
                            (this.Inicio.TimeOfDay < h.Inicio.TimeOfDay) ||
                            (this.Inicio.TimeOfDay > h.Fin.TimeOfDay) ||
                            (this.Fin.TimeOfDay < h.Inicio.TimeOfDay) ||
                            (this.Fin.TimeOfDay > h.Fin.TimeOfDay)
                    )
                    ||
                    diasElegidos.Any(e => diasDisponibles.All(d => d != e))
                )
                {
                    Dependencia.Avisar("Reserva fuera del horario de trabajo");
                    return;
                }

                await this.ServicioReservas.CrearAsync(new Reserva
                {
                    Id = Guid.NewGuid().ToString(),
                    Inicio = this.Inicio,
                    Fin = this.Fin,
                    EstacionamientoId = this.Id,
                    ClienteId = Application.Current.Properties[Propiedades.Id].ToString(),
                    Tolerancia = ((this.CajonesTotales - this.CajonesDisponibles) < 2) && this.Estacionamiento.Concurrido ? 5 : 15,
                    Disponible = true
                });

                this.Obtener();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
            finally
            {
                this.Habilitado = true;
                this.Ocupado = false;
            }
        }
    }

    public class ReservaActiva
    {
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public string Duracion { get; set; }
    }
}
