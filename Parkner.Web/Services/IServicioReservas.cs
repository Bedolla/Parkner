using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Parkner.Web.Services
{
    public interface IServicioReservas
    {
        Task<ListaPaginada<Reserva>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Reserva>> ObtenerDeAsync(PaginacionPeticion modelo);
        Task<Reserva> ObtenerAsync(string id);
        Task CrearAsync(Reserva modelo);
        Task EditarAsync(Reserva modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioReservas : IServicioReservas
    {
        public ServicioReservas
        (
            HttpClient cliente,
            ILogger<ServicioReservas> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioReservas> Registro { get; }

        public async Task<ListaPaginada<Reserva>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Reserva> reservas = await this.Cliente.PeticionGetAsync<ListaPaginada<Reserva>>("Reservas".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (reservas.Respuesta is null || reservas.Respuesta.Mensaje.Equals(String.Empty)) return reservas;

            this.Registro.LogError($"Error obteniendo reservas paginadas: {reservas.Respuesta.Mensaje}");
            throw new Exception(reservas.Respuesta.Mostrar ? reservas.Respuesta.Mensaje : "Error obteniendo reservas");
        }

        public async Task<ListaPaginada<Reserva>> ObtenerDeAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Reserva> reservas = await this.Cliente.PeticionGetAsync<ListaPaginada<Reserva>>("Reservas/De".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"Id", modelo.Id},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (reservas.Respuesta is null || reservas.Respuesta.Mensaje.Equals(String.Empty)) return reservas;

            this.Registro.LogError($"Error obteniendo reservas paginadas del responsable {modelo.Id}: {reservas.Respuesta.Mensaje}");
            throw new Exception(reservas.Respuesta.Mostrar ? reservas.Respuesta.Mensaje : "Error obteniendo reservas");
        }

        public async Task<Reserva> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Reserva reserva = await this.Cliente.PeticionGetAsync<Reserva>($"Reservas/{id}");

            if (reserva.Respuesta is null || reserva.Respuesta.Mensaje.Equals(String.Empty)) return reserva;

            this.Registro.LogError($"Error obteniendo reserva: {reserva.Respuesta.Mensaje}");
            throw new Exception(reserva.Respuesta.Mostrar ? reserva.Respuesta.Mensaje : "Error obteniendo reserva");
        }

        public async Task CrearAsync(Reserva modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Reservas", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando reserva: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando reserva");
        }

        public async Task EditarAsync(Reserva modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Reservas", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando reserva: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando reserva");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Reservas/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando reserva: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando reserva");
        }
    }
}
