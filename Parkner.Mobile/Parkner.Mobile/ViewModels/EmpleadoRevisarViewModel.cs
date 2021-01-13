using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class EmpleadoRevisarViewModel : BaseViewModel
    {
        private bool _habilitado;
        private string _id;
        private Command _registrarCommand;

        public EmpleadoRevisarViewModel
        (
            IServicioReservas servicioReservas
        )
        {
            this.ServicioReservas = servicioReservas;

            this.Habilitado = true;
        }

        public EmpleadoRevisarViewModel() { }

        private IServicioReservas ServicioReservas { get; }

        public bool Habilitado
        {
            get => this._habilitado;
            set
            {
                this._habilitado = value;
                this.OnPropertyChanged();
            }
        }

        public string Id
        {
            get => this._id;
            set
            {
                this._id = value;
                this.OnPropertyChanged();
            }
        }

        public Command RegistrarCommand
        {
            get => this._registrarCommand ??= new Command(this.Registrar);
            set => this._registrarCommand = value;
        }

        private async void Registrar()
        {
            try
            {
                if
                (
                    this.Id.EsNulo()
                )
                {
                    Dependencia.Avisar("Debe ingresar el código de reserva");
                    return;
                }

                this.Habilitado = false;
                this.Ocupado = true;

                Reserva reserva = await this.ServicioReservas.ObtenerAsync(this.Id);

                if
                (
                    reserva.Estacionamiento.Empleados.All(e => e.Id != Application.Current.Properties[Propiedades.Id].ToString())
                )
                {
                    Dependencia.Avisar("Esta reserva pertenece a un estacionamiento en el que usted no labora");
                    return;
                }

                decimal costoPorMinuto = reserva.Estacionamiento.Costo / 60;

                if (reserva.EmpleadoInicializadorId.EsNulo())
                {
                    reserva.EmpleadoInicializadorId = Application.Current.Properties[Propiedades.Id].ToString();
                    reserva.Inicio = DateTime.Now;
                }
                else
                {
                    decimal? cobrado = (decimal?)Math.Ceiling((DateTime.Now - reserva.Inicio).TotalMinutes * (double)costoPorMinuto);
                    reserva.EmpleadoFinalizadorId = Application.Current.Properties[Propiedades.Id].ToString();
                    reserva.Fin = DateTime.Now;
                    reserva.Cobrado = cobrado < 5 ? 5 : cobrado;
                    reserva.Disponible = false;
                }

                await this.ServicioReservas.EditarAsync(reserva);

                this.Ocupado = false;
                this.Id = String.Empty;
                Dependencia.Avisar("Reserva registrada");
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
            finally
            {
                this.Ocupado = false;
                this.Habilitado = true;
            }
        }
    }
}
