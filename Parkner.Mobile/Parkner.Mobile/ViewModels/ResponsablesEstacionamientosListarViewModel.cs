using Parkner.Core.Constants;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace Parkner.Mobile.ViewModels
{
    public class ResponsablesEstacionamientosListarViewModel : BaseViewModel
    {
        private ICommand _crearCommand;
        private ObservableCollection<Estacionamiento> _estacionamientos;
        private ICommand _tapCommand;

        public ResponsablesEstacionamientosListarViewModel
        (
            IServicioEstacionamientos servicioEstacionamientos
        ) =>
            this.ServicioEstacionamientos = servicioEstacionamientos;

        public ResponsablesEstacionamientosListarViewModel() { }

        private IServicioEstacionamientos ServicioEstacionamientos { get; }

        public ObservableCollection<Estacionamiento> Estacionamientos
        {
            get => this._estacionamientos ??= new ObservableCollection<Estacionamiento>();
            set
            {
                this._estacionamientos = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand TapCommand
        {
            get => this._tapCommand ??= new Command<ItemTappedEventArgs>(this.ReservaSeleccionada);
            set => this._tapCommand = value;
        }

        public ICommand CrearCommand
        {
            get => this._crearCommand ??= new Command(this.Crear);
            set => this._crearCommand = value;
        }

        private async void Crear()
        {
            await Dependencia.Navegacion.PushAsync(Dependencia.Obtener<ResponsablesEstacionamientoCrearPage>());
        }

        private async void ReservaSeleccionada(ItemTappedEventArgs argumentos)
        {
            if (argumentos.ItemData is Estacionamiento estacionamiento) await Dependencia.Navegacion.PushAsync(Dependencia.Obtener<ResponsablesEstacionamientoVerPage>(estacionamiento.Id));
        }

        private async void Recibir()
        {
            try
            {
                ListaPaginada<Estacionamiento> estacionamientosPaginados = await this.ServicioEstacionamientos.ObtenerDeAsync(Application.Current.Properties[Propiedades.Id].ToString());
                estacionamientosPaginados.Lista.ForEach(e =>
                {
                    e.Direccion.Calle = $"{e.Direccion.Calle} {e.Direccion.Numero}, {e.Direccion.Colonia}";
                    e.Foto = $"{Uris.Fotos}{e.Foto}";
                });
                this.Estacionamientos = new ObservableCollection<Estacionamiento>(estacionamientosPaginados.Lista);
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }

        public override void Cablear()
        {
            this.Recibir();
        }
    }
}
