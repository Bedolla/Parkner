using Parkner.Mobile.Models;
using Parkner.Mobile.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapaPage : ContentPage
    {
        public MapaPage()
        {
            this.InitializeComponent();

            //((MapaViewModel)this.BindingContext).InicializarMapa(this.Mapa);
        }
    }

    internal class MapaViewModel : BaseViewModel
    {
        private double _latitud;
        private double _longitud;
        private ICommand _mapaCliqueadoCommand;
        private ObservableCollection<Pin> _pines;
        private Posicion _posision;

        public double Latitud
        {
            get => this._latitud;
            set
            {
                this._latitud = value;
                this.OnPropertyChanged();
            }
        }

        public double Longitud
        {
            get => this._longitud;
            set
            {
                this._longitud = value;
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

        public ICommand MapaCliqueadoCommand
        {
            get => this._mapaCliqueadoCommand ??= new Command<Position>(this.MapaCliqueado);
            set => this._mapaCliqueadoCommand = value;
        }

        private void MapaCliqueado(Position posicion)
        {
            this.Pines.Clear();

            Pin pin = new Pin
            {
                AutomationId = "Estacionamiento.Id",
                Label = "Estacionamiento.Nombre",
                Address = "Estacionamiento.Direccion",
                Position = posicion
            };

            pin.InfoWindowClicked += async (sender, args) =>
            {
                Pin pinCliqueado = (Pin)sender;

                await Application.Current.MainPage.DisplayAlert("Aviso", $"{pinCliqueado.Label}, está a {8 * 1000:N2} metros de ti", "Entendido");

                //await Dependencia.Navegacion.PushAsync(Dependencia.Obtener<EstacionamientosVerPage>(pin.AutomationId));
            };

            this.Pines.Add(pin);

            this.Latitud = posicion.Latitude;
            this.Longitud = posicion.Longitude;
        }

        public override async void Cablear()
        {
            Location ubicacion = (await Geocoding.GetLocationsAsync("Felipe Sevilla del Río 456, Lomas de Circunvalación, Colima")).FirstOrDefault();

            if (ubicacion is null) return;

            Placemark lugar = (await Geocoding.GetPlacemarksAsync(ubicacion.Latitude, ubicacion.Longitude)).FirstOrDefault();

            if (lugar is null) return;

            string datosDelLugar = $"{nameof(lugar.AdminArea)}: {lugar.AdminArea}\n" +
                                   $"{nameof(lugar.CountryCode)}: {lugar.CountryCode}\n" +
                                   $"{nameof(lugar.CountryName)}: {lugar.CountryName}\n" +
                                   $"{nameof(lugar.FeatureName)}: {lugar.FeatureName}\n" +
                                   $"{nameof(lugar.Locality)}: {lugar.Locality}\n" +
                                   $"{nameof(lugar.PostalCode)}: {lugar.PostalCode}\n" +
                                   $"{nameof(lugar.SubAdminArea)}: {lugar.SubAdminArea}\n" +
                                   $"{nameof(lugar.SubLocality)}: {lugar.SubLocality}\n" +
                                   $"{nameof(lugar.SubThoroughfare)}: {lugar.SubThoroughfare}\n" +
                                   $"{nameof(lugar.Thoroughfare)}: {lugar.Thoroughfare}\n";

            this.Posision = new Posicion(ubicacion.Latitude, ubicacion.Longitude, 50);
        }

        public override void Descablear()
        {
            base.Descablear();
        }
    }
}
