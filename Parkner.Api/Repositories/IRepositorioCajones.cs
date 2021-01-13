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
    public interface IRepositorioCajones
    {
        Task<ListaPaginada<Cajon>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Cajon>> DeAsync(PaginacionPeticion modelo);
        Task<Cajon> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Cajon modelo);
        Task<Respuesta> EditarAsync(Cajon modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioCajones : IRepositorioCajones
    {
        public RepositorioCajones
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

        public async Task<ListaPaginada<Cajon>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Cajon> cajones = await this.Contexto.Cajones
                                            .Buscar(modelo.TerminoBuscado)
                                            .Ordenar(modelo.OrdenarPor)
                                            .ToListAsync();

            int cantidadTotal = cajones.Count;

            return new ListaPaginada<Cajon> {Lista = cajones.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<ListaPaginada<Cajon>> DeAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Cajon> cajones = await this.Contexto.Cajones
                                            .Where(c => c.EstacionamientoId == modelo.Id)
                                            .Buscar(modelo.TerminoBuscado)
                                            .Ordenar(modelo.OrdenarPor)
                                            .ToListAsync();

            int cantidadTotal = cajones.Count;

            return new ListaPaginada<Cajon> {Lista = cajones.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Cajon> ObtenerAsync(string id) => await this.Contexto.Cajones.FindAsync(id) ?? throw new Exception("Cajón no encontrado");

        public async Task<Respuesta> CrearAsync(Cajon modelo)
        {
            if
            (
                await this.Contexto.Cajones
                          .AnyAsync
                          (c =>
                              (c.Nombre == modelo.Nombre) &&
                              (c.EstacionamientoId == modelo.EstacionamientoId)
                          )
            )
                throw new Exception("Cajón ya registrado");

            await this.Contexto.Cajones.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Cajon modelo)
        {
            try
            {
                Cajon cajon = await this.Contexto.Cajones.FindAsync(modelo.Id);

                if (cajon is null) throw new Exception("Cajón no encontrado");

                cajon.Nombre = modelo.Nombre;
                cajon.Disponible = modelo.Disponible;

                if
                (
                    await this.Contexto.Cajones
                              .AnyAsync
                              (c =>
                                  (c.Nombre == cajon.Nombre) &&
                                  (c.EstacionamientoId == cajon.EstacionamientoId) &&
                                  (c.Id != cajon.Id)
                              )
                )
                    throw new Exception("Cajón ya registrado");

                this.Contexto.Cajones.Update(cajon);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Cajones.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El cajón ya no existe");
                throw new Exception("El cajón fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Cajon cajon = await this.Contexto.Cajones.FindAsync(id);

                if (cajon is null) throw new Exception("Cajón no encontrado");

                this.Contexto.Cajones.Remove(cajon);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Cajones.AllAsync(r => r.Id != id)) throw new Exception("El cajón ya no existe");
                throw new Exception("El cajón fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }
    }
}
