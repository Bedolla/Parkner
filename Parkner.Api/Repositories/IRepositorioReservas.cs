using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parkner.Api.Models;
using Parkner.Core.Constants;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Api.Repositories
{
    public interface IRepositorioReservas
    {
        Task<ListaPaginada<Reserva>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Reserva>> DeAsync(PaginacionPeticion modelo);
        Task<Reserva> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Reserva modelo);
        Task<Respuesta> EditarAsync(Reserva modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioReservas : IRepositorioReservas
    {
        public RepositorioReservas
        (
            Contexto contexto,
            IMapper mapeador
        )
        {
            this.Contexto = contexto;
            this.Mapeador = mapeador;
        }

        private Contexto Contexto { get; }
        private IMapper Mapeador { get; }

        public async Task<ListaPaginada<Reserva>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Reserva> reservas = await this.Contexto.Reservas
                                               .Include(r => r.Cliente)
                                               .Buscar(modelo.TerminoBuscado)
                                               .Ordenar(modelo.OrdenarPor)
                                               .ToListAsync();

            int cantidadTotal = reservas.Count;

            return new ListaPaginada<Reserva> {Lista = reservas.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<ListaPaginada<Reserva>> DeAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Reserva> reservas = await this.Contexto.Reservas
                                               .Include(r => r.Cliente)
                                               .Include(r => r.Estacionamiento).ThenInclude(e => e.Direccion)
                                               .Include(r => r.EmpleadoInicializador)
                                               .Include(r => r.EmpleadoFinalizador)
                                               .Where(r => (r.EstacionamientoId == modelo.Id) || (r.ClienteId == modelo.Id) /*&& (r.Fin is null)*/)
                                               .Buscar(modelo.TerminoBuscado)
                                               .Ordenar(modelo.OrdenarPor)
                                               .ToListAsync();

            int cantidadTotal = reservas.Count;

            return new ListaPaginada<Reserva> {Lista = reservas.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Reserva> ObtenerAsync(string id) =>
            await this.Contexto.Reservas
                      .Include(r => r.Cliente)
                      .Include(r => r.Estacionamiento).ThenInclude(e => e.Direccion )
                      .Include(r => r.Estacionamiento).ThenInclude(e => e.Empleados)
                      .Include(r => r.EmpleadoInicializador)
                      .Include(r => r.EmpleadoFinalizador)
                      .FirstOrDefaultAsync(r => r.Id == id) ?? throw new Exception("Reserva no encontrada");

        public async Task<Respuesta> CrearAsync(Reserva modelo)
        {
            await this.Contexto.Reservas.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Reserva modelo)
        {
            try
            {
                Reserva reserva = await this.Contexto.Reservas.FindAsync(modelo.Id);

                if (reserva is null) throw new Exception("Reserva no encontrada");

                reserva.Inicio = modelo.Inicio;
                reserva.Fin = modelo.Fin;
                reserva.EmpleadoFinalizadorId = modelo.EmpleadoFinalizadorId;
                reserva.EmpleadoInicializadorId = modelo.EmpleadoInicializadorId;
                reserva.Cobrado = modelo.Cobrado;
                reserva.Disponible = modelo.Disponible;
                reserva.ClienteId = modelo.ClienteId;
                reserva.EstacionamientoId = modelo.EstacionamientoId;

                this.Contexto.Reservas.Update(reserva);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Reservas.AllAsync(r => r.Id != modelo.Id)) throw new Exception("La reserva ya no existe");
                throw new Exception("La reserva fue modificada por alguien más mientras usted trataba de editarla");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Reserva reserva = await this.Contexto.Reservas.FindAsync(id);

                if (reserva is null) throw new Exception("Reserva no encontrada");

                this.Contexto.Reservas.Remove(reserva);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Reservas.AllAsync(r => r.Id != id)) throw new Exception("La reserva ya no existe");
                throw new Exception("La reserva fue modificada por alguien más mientras usted trataba de editarla");
            }
        }
    }
}
