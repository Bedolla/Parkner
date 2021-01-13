using Parkner.Core.Constants;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class ResponsablesGananciasViewModel : BaseViewModel
    {
        private ObservableCollection<Ganancia> _ganancias;

        public ResponsablesGananciasViewModel
        (
            IServicioGanancias servicioGanancias
        ) =>
            this.ServicioGanancias = servicioGanancias;

        public ResponsablesGananciasViewModel() { }

        private IServicioGanancias ServicioGanancias { get; }

        public ObservableCollection<Ganancia> Ganancias
        {
            get => this._ganancias ??= new ObservableCollection<Ganancia>();
            set
            {
                this._ganancias = value;
                this.OnPropertyChanged();
            }
        }

        public override void Cablear()
        {
            this.Obtener();
        }

        private async void Obtener()
        {
            try
            {
                ListaPaginada<Ganancia> gananciasPaginadas = await this.ServicioGanancias.ObtenerDeAsync(Application.Current.Properties[Propiedades.Id].ToString());
                gananciasPaginadas.Lista.ForEach(g => this.Ganancias.Add(g));
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }
    }
}
