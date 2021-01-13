using Microsoft.Extensions.DependencyInjection;
using Parkner.Core.Constants;
using Parkner.Data.Entities;
using Parkner.Mobile.ViewModels;
using Parkner.Mobile.Views;
using System;
using Xamarin.Forms;

namespace Parkner.Mobile.Services
{
    public static class Dependencia
    {
        private static IServiceProvider Proveedor { get; set; }

        public static Page Inicio
        {
            get => Application.Current.MainPage;
            set => Application.Current.MainPage = value;
        }

        public static INavigation Navegacion => Application.Current.MainPage is MasterDetailPage masta ? masta.Detail.Navigation : Application.Current.MainPage.Navigation;

        public static async void Avisar(string mensaje, string titulo = "Aviso", string boton = "Entendido")
        {
            await Application.Current.MainPage.DisplayAlert(titulo, mensaje, boton);
        }

        public static T Obtener<T>(params object[] parametros) where T : class =>
            parametros is null ?
                ActivatorUtilities.CreateInstance<T>(Dependencia.Proveedor) :
                ActivatorUtilities.CreateInstance<T>(Dependencia.Proveedor, parametros);

        private static IServiceCollection ConfigurarServicios(this IServiceCollection servicios)
        {
            servicios.AddHttpClient<IServicioSesion, ServicioSesion>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioUsuarios, ServicioUsuarios>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioClientes, ServicioClientes>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioResponsables, ServicioResponsables>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioGanancias, ServicioGanancias>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioEmpleados, ServicioEmpleados>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioEstacionamientos, ServicioEstacionamientos>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioHorarios, ServicioHorarios>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioCajones, ServicioCajones>(c => c.BaseAddress = new Uri(Uris.Api));
            servicios.AddHttpClient<IServicioReservas, ServicioReservas>(c => c.BaseAddress = new Uri(Uris.Api));

            servicios.AddTransient<Usuario>();
            servicios.AddTransient<Cliente>();
            servicios.AddTransient<Responsable>();
            servicios.AddTransient<Reserva>();
            servicios.AddTransient<Empleado>();
            servicios.AddTransient<Estacionamiento>();
            servicios.AddTransient<Ganancia>();
            servicios.AddTransient<Cajon>();
            servicios.AddTransient<Horario>();
            servicios.AddTransient<Direccion>();

            servicios.AddTransient<RegistrarOriginalPage>();
            servicios.AddTransient<IngresarOriginalPage>();
            servicios.AddTransient<SalirPage>();
            servicios.AddTransient<LeerBorrarPage>();
            servicios.AddTransient<CrearEditarPage>();

            servicios.AddTransient<RegistrarOriginalViewModel>();
            servicios.AddTransient<IngresarViewModel>();
            servicios.AddTransient<IngresarOriginalViewModel>();
            servicios.AddTransient<SalirViewModel>();
            servicios.AddTransient<ClienteEstacionamientosListarViewModel>();
            servicios.AddTransient<LeerBorrarViewModel>();
            servicios.AddTransient<CrearEditarViewModel>();

            return servicios;
        }

        public static void Inicializar() =>
            Dependencia.Proveedor = new ServiceCollection()
                                    .ConfigurarServicios()
                                    .BuildServiceProvider();
    }
}
