using Parkner.Mobile.Models;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace Parkner.Mobile.ViewModels
{
    public class ClienteEstacionamientosListarViewModel : BaseViewModel
    {
        private ObservableCollection<Pin> _pines;

        private Posicion _posision;

        public ClienteEstacionamientosListarViewModel
        (
            IServicioEstacionamientos servicioEstacionamientos
        )
        {
            this.ServicioEstacionamientos = servicioEstacionamientos;

            this.Obtener();
        }

        public ClienteEstacionamientosListarViewModel() { }

        private IServicioEstacionamientos ServicioEstacionamientos { get; }

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

        private async void Obtener()
        {
            Location ubicacion = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            this.Posision = new Posicion(ubicacion.Latitude, ubicacion.Longitude);

            this.Pines.Clear();

            (await this.ServicioEstacionamientos.ObtenerTodosAsync()).Lista.ForEach(e =>
            {
                Pin pin = new()
                {
                    AutomationId = e.Id,
                    Position = new Position(Convert.ToDouble(e.Direccion.Latitud), Convert.ToDouble(e.Direccion.Longitud)),
                    Type = PinType.Place,
                    Label = e.Nombre,
                    Address = $"{e.Direccion.Calle} {e.Direccion.Numero}, {e.Direccion.Colonia}"
                };

                pin.InfoWindowClicked += async (_, _) =>
                    //Pin pinCliqueado = (Pin)t;

                    //double kilometros = Location.CalculateDistance(new Location(ubicacion.Latitude, ubicacion.Longitude), new Location(Convert.ToDouble(e.Direccion.Latitud), Convert.ToDouble(e.Direccion.Longitud)), DistanceUnits.Kilometers);

                    //await Application.Current.MainPage.DisplayAlert("Aviso", $"{pinCliqueado.Label}, está a {kilometros * 1000:N2} metros de ti", "Entendido");
                    await Dependencia.Navegacion.PushAsync(Dependencia.Obtener<ClienteEstacionamientosVerPage>(pin.AutomationId));

                this.Pines.Add(pin);
            });
        }
    }
}
