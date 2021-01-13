using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.SessionStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using Serilog;
using Serilog.Events;
using System;
using System.Text;

namespace Parkner.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuracion) => this.Configuracion = configuracion;

        private IConfiguration Configuracion { get; }

        public void ConfigureServices(IServiceCollection servicios)
        {
            servicios.AddRazorPages();
            servicios.AddServerSideBlazor();

            servicios.AddBlazorise(o => o.ChangeTextOnKeyPress = true)
                     .AddBootstrapProviders()
                     .AddFontAwesomeIcons();

            servicios.AddBlazoredModal();
            servicios.AddBlazoredSessionStorage();

            servicios.AddHttpClient<IServicioSesion, ServicioSesion>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioUsuarios, ServicioUsuarios>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioClientes, ServicioClientes>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioEmpleados, ServicioEmpleados>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioResponsables, ServicioResponsables>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioEstacionamientos, ServicioEstacionamientos>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioGanancias, ServicioGanancias>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioCajones, ServicioCajones>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioHorarios, ServicioHorarios>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));
            servicios.AddHttpClient<IServicioReservas, ServicioReservas>(c => c.BaseAddress = new Uri(this.Configuracion["ApiRuta"]));

            servicios.AddScoped<AuthenticationStateProvider, ProveedorDeEstadoDeAutenticacion>();
            servicios.AddScoped<IModalService, ModalService>();
            servicios.AddScoped<IMensajes, Mensajes>();
            servicios.AddScoped<IApi, Api>();

            servicios.AddSingleton<UsuarioState>();
            servicios.AddSingleton<ResponsableState>();
            servicios.AddSingleton<EstacionamientoState>();

            servicios.AddLogging
            (b =>
                b.AddSerilog
                (new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .WriteTo.File
                 (
                     "Logs//.txt",
                     LogEventLevel.Error,
                     rollingInterval: RollingInterval.Day,
                     encoding: Encoding.UTF8,
                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                 )
                 .CreateLogger()
                )
            );
        }

        public void Configure(IApplicationBuilder aplicacion, IWebHostEnvironment entorno)
        {
            if (entorno.IsDevelopment())
            {
                aplicacion.UseDeveloperExceptionPage();
            }
            else
            {
                aplicacion.UseExceptionHandler("/Error");
                aplicacion.UseHsts();
            }

            aplicacion.UseHttpsRedirection();
            aplicacion.UseStaticFiles();

            aplicacion.UseRouting();
            aplicacion.UseAuthentication();
            aplicacion.UseAuthorization();

            aplicacion.ApplicationServices
               .UseBootstrapProviders()
               .UseFontAwesomeIcons();

            aplicacion.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
