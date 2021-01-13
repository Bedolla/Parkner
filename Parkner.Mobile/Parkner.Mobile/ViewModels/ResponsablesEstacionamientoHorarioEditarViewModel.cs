using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class ResponsablesEstacionamientoHorarioEditarViewModel : BaseViewModel
    {
        private ICommand _borrarCommand;
        private string _dia;
        private ICommand _editarCommand;
        private TimeSpan _fin;
        private TimeSpan _inicio;

        public ResponsablesEstacionamientoHorarioEditarViewModel
        (
            IServicioHorarios servicioHorarios,
            string id
        )
        {
            this.ServicioHorarios = servicioHorarios;
            this.Id = id;

            this.Recibir();
        }

        public ResponsablesEstacionamientoHorarioEditarViewModel() { }

        private IServicioHorarios ServicioHorarios { get; }

        private string Id { get; }

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

        public ICommand EditarCommand
        {
            get => this._editarCommand ??= new Command(this.Enviar);
            set => this._editarCommand = value;
        }

        public ICommand BorrarCommand
        {
            get => this._borrarCommand ??= new Command(this.Borrar);
            set => this._borrarCommand = value;
        }

        private async void Recibir()
        {
            this.Horario = await this.ServicioHorarios.ObtenerAsync(this.Id);
            this.Dia = ((Dias)this.Horario.DiaNumero).ToString();
            this.Inicio = this.Horario.Inicio.TimeOfDay;
            this.Fin = this.Horario.Fin.TimeOfDay;
        }

        private async void Enviar()
        {
            try
            {
                this.Horario.Dia = this.Dia;
                this.Horario.DiaNumero = Convert.ToInt32(Enum.Parse(typeof(Dias), this.Dia));
                this.Horario.Inicio = DateTime.Today.Add(this.Inicio);
                this.Horario.Fin = DateTime.Today.Add(this.Fin);

                await this.ServicioHorarios.EditarAsync(this.Horario);

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }

        private async void Borrar()
        {
            try
            {
                await this.ServicioHorarios.BorrarAsync(this.Id);

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }
    }
}
