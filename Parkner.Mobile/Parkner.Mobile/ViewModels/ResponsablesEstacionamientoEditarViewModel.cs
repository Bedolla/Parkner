using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Models;
using Parkner.Mobile.Services;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ResponsablesEstacionamientoEditarViewModel : BaseViewModel
    {
        private string _calle;
        private string _codigoPostal;
        private string _colonia;
        private decimal _costo;
        private string _descripcion;
        private string _entreCalles;
        private double _latitud;
        private double _longitud;
        private ICommand _mapaCliqueadoCommand;
        private string _municipio;
        private string _nombre;
        private string _numero;
        private ObservableCollection<Pin> _pines;
        private Posicion _posision;
        private string _tipo;

        public ResponsablesEstacionamientoEditarViewModel
        (
            IServicioEstacionamientos servicioEstacionamientos,
            string estacionamientoId
        )
        {
            this.ServicioEstacionamientos = servicioEstacionamientos;
            this.EstacionamientoId = estacionamientoId;
            this.RegistrarCommand = new Command(this.Editar);
            this.BuscarFotoCommand = new Command<Grid>(this.BuscarFoto);
        }

        public ResponsablesEstacionamientoEditarViewModel() { }

        private IServicioEstacionamientos ServicioEstacionamientos { get; }
        private string EstacionamientoId { get; }

        public bool Habilitado { get; set; }
        public Command RegistrarCommand { get; set; }
        public ICommand BuscarFotoCommand { get; set; }

        public string Nombre
        {
            get => this._nombre;
            set
            {
                this._nombre = value;
                this.OnPropertyChanged();
            }
        }

        public string Descripcion
        {
            get => this._descripcion;
            set
            {
                this._descripcion = value;
                this.OnPropertyChanged();
            }
        }

        public decimal Costo
        {
            get => this._costo;
            set
            {
                this._costo = value;
                this.OnPropertyChanged();
            }
        }

        public string Tipo
        {
            get => this._tipo;
            set
            {
                this._tipo = value;
                this.OnPropertyChanged();
            }
        }

        public string Numero
        {
            get => this._numero;
            set
            {
                this._numero = value;
                this.OnPropertyChanged();
            }
        }

        public string Calle
        {
            get => this._calle;
            set
            {
                this._calle = value;
                this.OnPropertyChanged();
            }
        }

        public string EntreCalles
        {
            get => this._entreCalles;
            set
            {
                this._entreCalles = value;
                this.OnPropertyChanged();
            }
        }

        public string Colonia
        {
            get => this._colonia;
            set
            {
                this._colonia = value;
                this.OnPropertyChanged();
            }
        }

        public string CodigoPostal
        {
            get => this._codigoPostal;
            set
            {
                this._codigoPostal = value;
                this.OnPropertyChanged();
            }
        }

        public string Municipio
        {
            get => this._municipio;
            set
            {
                this._municipio = value;
                this.OnPropertyChanged();
            }
        }

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

        private byte[] FotoBytes { get; set; }

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

        public Estacionamiento Estacionamiento { get; set; }

        private async void MapaCliqueado(Position posicion)
        {
            this.Pines.Clear();

            this.Latitud = posicion.Latitude;
            this.Longitud = posicion.Longitude;

            this.Pines.Add(new Pin
            {
                Label = "Estacionamiento por crear",
                Position = posicion
            });

            try
            {
                Placemark lugar = (await Geocoding.GetPlacemarksAsync(posicion.Latitude, posicion.Longitude)).FirstOrDefault();

                if (lugar is null) return;

                if (this.Numero.EsNulo()) this.Numero = lugar.SubThoroughfare;
                if (this.Calle.EsNulo()) this.Calle = lugar.Thoroughfare;
                if (this.Colonia.EsNulo()) this.Colonia = lugar.SubLocality;
                if (this.CodigoPostal.EsNulo()) this.CodigoPostal = lugar.PostalCode;
                if (this.Municipio.EsNulo()) this.Municipio = lugar.Locality;
            }
            catch (Exception)
            {
                Dependencia.Avisar("El servicio de geolocalización no está disponible temporalmente en tu zona");
            }
        }

        private async void BuscarFoto(Grid contendorEditor)
        {
            try
            {
                this.Habilitado = false;

                Stream fotoPorSubir = await DependencyService.Get<IPhotoPickerService>().ObtnerImagenAsync();

                if (fotoPorSubir == null) return;

                SfImageEditor editor = new();
                editor.ImageSaving += async (_, a) =>
                {
                    a.Cancel = true;

                    using Image fotoTemporal = await Image.LoadAsync(a.Stream);
                    using Image clon = fotoTemporal.Clone
                    (
                        c => c.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.BoxPad,
                            Size = new Size(256, 256),
                            Position = AnchorPositionMode.Center
                        })
                    );
                    MemoryStream fotoMemoria = new();
                    await clon.SaveAsync(fotoMemoria, new PngEncoder {CompressionLevel = PngCompressionLevel.NoCompression, TransparentColorMode = PngTransparentColorMode.Preserve});
                    //string fotoBase64String = $"data:image/png;base64,{Convert.ToBase64String(fotoMemoria.ToArray())}";
                    //ImageSource imagen = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(fotoBase64String)));
                    this.FotoBytes = fotoMemoria.ToArray();

                    contendorEditor.HeightRequest = 128;
                    contendorEditor.VerticalOptions = LayoutOptions.Start;
                    if (Device.RuntimePlatform == Device.UWP)
                        editor.IsVisible = false;
                    else
                        contendorEditor.Children.Clear();
                    contendorEditor.Children.Add(new Xamarin.Forms.Image {WidthRequest = 128, HeightRequest = 128, Source = ImageSource.FromStream(() => new MemoryStream(this.FotoBytes))});
                };
                editor.Source = ImageSource.FromStream(() => fotoPorSubir);
                editor.SetToolbarItemVisibility("Undo,Redo,Text,Shape,Path,free,original,3:1,3:2,4:3,5:4,16:9,Effects", false);
                editor.ToolbarSettings.ToolbarItems.ForEach(i =>
                    i.Text = i.Name switch
                    {
                        "Save" => "Guardar",
                        "Reset" => "Deshacer",
                        _ => i.Text
                    }
                );

                contendorEditor.IsVisible = true;
                contendorEditor.HeightRequest = 512;
                contendorEditor.VerticalOptions = LayoutOptions.Start;
                contendorEditor.Children.Clear();
                contendorEditor.Children.Add(editor);
            }
            catch (Exception excepcion)
            {
                Debug.WriteLine(excepcion.Message);
            }
            finally
            {
                this.Habilitado = true;
            }
        }

        private async void Editar()
        {
            try
            {
                if
                (
                    this.Nombre.EsNulo() ||
                    this.Descripcion.EsNulo() ||
                    (this.Costo < 1) ||
                    this.Tipo.EsNulo() ||
                    this.Numero.EsNulo() ||
                    this.Calle.EsNulo() ||
                    this.EntreCalles.EsNulo() ||
                    this.Colonia.EsNulo() ||
                    this.CodigoPostal.EsNulo() ||
                    this.Municipio.EsNulo() ||
                    (this.Latitud == default) ||
                    (this.Longitud == default)
                )
                {
                    Dependencia.Avisar("Debe llenar todos los campos");
                    return;
                }

                this.Habilitado = false;
                this.Ocupado = true;

                Estacionamiento estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.EstacionamientoId);

                string nombreFoto = $"{estacionamiento.Id}.png";

                if (this.FotoBytes is not null) await this.FotoBytes.SubirFotoAsync(nombreFoto, "Estacionamiento");

                estacionamiento.Nombre = this.Nombre;
                estacionamiento.Descripcion = this.Descripcion;
                estacionamiento.Costo = this.Costo;
                estacionamiento.Tipo = this.Tipo;
                estacionamiento.Direccion.Numero = this.Numero;
                estacionamiento.Direccion.Calle = this.Calle;
                estacionamiento.Direccion.EntreCalles = this.EntreCalles;
                estacionamiento.Direccion.Colonia = this.Colonia;
                estacionamiento.Direccion.CodigoPostal = this.CodigoPostal;
                estacionamiento.Direccion.Municipio = this.Municipio;
                estacionamiento.Direccion.Latitud = this.Latitud.ToString(CultureInfo.CurrentCulture);
                estacionamiento.Direccion.Longitud = this.Longitud.ToString(CultureInfo.CurrentCulture);
                estacionamiento.Foto = $"images/avatares/estacionamientos/{nombreFoto}";

                await this.ServicioEstacionamientos.EditarAsync(estacionamiento);

                this.Ocupado = false;
                this.Habilitado = true;

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", excepcion.Message, "Entendido");
            }
            finally
            {
                this.Ocupado = false;
                this.Habilitado = true;
            }
        }

        private async void Recibir()
        {
            this.Habilitado = true;

            //Location ubicacion = (await Geocoding.GetLocationsAsync("Felipe Sevilla del Río 456, Lomas de Circunvalación, Colima")).FirstOrDefault();

            //if (ubicacion is null) return;

            //Placemark lugar = (await Geocoding.GetPlacemarksAsync(ubicacion.Latitude, ubicacion.Longitude)).FirstOrDefault();

            //if (lugar is null) return;

            //string datosDelLugar = $"Estado: {lugar.AdminArea}\n" +
            //                       $"Código de país: {lugar.CountryCode}\n" +
            //                       $"País: {lugar.CountryName}\n" +
            //                       $"Número exterior: {lugar.FeatureName}\n" +
            //                       $"Municipio: {lugar.Locality}\n" +
            //                       $"Código postal: {lugar.PostalCode}\n" +
            //                       $"Condado: {lugar.SubAdminArea}\n" +
            //                       $"Colonia: {lugar.SubLocality}\n" +
            //                       $"Número exterior/interior: {lugar.SubThoroughfare}\n" +
            //                       $"Calle: {lugar.Thoroughfare}\n";

            this.Estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.EstacionamientoId);
            this.Nombre = this.Estacionamiento.Nombre;
            this.Descripcion = this.Estacionamiento.Descripcion;
            this.Costo = this.Estacionamiento.Costo;
            this.Tipo = this.Estacionamiento.Tipo;
            this.Numero = this.Estacionamiento.Direccion.Numero;
            this.Calle = this.Estacionamiento.Direccion.Calle;
            this.EntreCalles = this.Estacionamiento.Direccion.EntreCalles;
            this.Colonia = this.Estacionamiento.Direccion.Colonia;
            this.CodigoPostal = this.Estacionamiento.Direccion.CodigoPostal;
            this.Municipio = this.Estacionamiento.Direccion.Municipio;
            this.Latitud = Convert.ToDouble(this.Estacionamiento.Direccion.Latitud);
            this.Longitud = Convert.ToDouble(this.Estacionamiento.Direccion.Longitud);

            this.Posision = new Posicion(this.Latitud, this.Longitud, 500);
            this.Pines.Clear();
            this.Pines.Add(new Pin
            {
                Label = this.Estacionamiento.Nombre,
                Address = $"{this.Estacionamiento.Direccion.Calle} {this.Estacionamiento.Direccion.Numero}, {this.Estacionamiento.Direccion.Colonia}",
                Position = new Position(this.Posision.Latitud, this.Posision.Longitud),
                Type = PinType.Place
            });
        }

        public override void Cablear()
        {
            this.Recibir();
        }
    }
}
