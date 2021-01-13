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
    public interface IRepositorioGanancias
    {
        Task<ListaPaginada<Ganancia>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Ganancia>> DeAsync(PaginacionPeticion modelo);
        Task<Ganancia> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Ganancia modelo);
        Task<Respuesta> EditarAsync(Ganancia modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioGanancias : IRepositorioGanancias
    {
        public RepositorioGanancias
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

        public async Task<ListaPaginada<Ganancia>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Ganancia> ganancias = await this.Contexto.Ganancias
                                                 .Ordenar(modelo.OrdenarPor)
                                                 .ToListAsync();

            int cantidadTotal = ganancias.Count;

            return new ListaPaginada<Ganancia> {Lista = ganancias.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<ListaPaginada<Ganancia>> DeAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Ganancia> ganancias = await this.Contexto.Ganancias
                                                 .Include(e => e.Responsable)
                                                 .Where(e => e.ResponsableId == modelo.Id)
                                                 .Ordenar(modelo.OrdenarPor)
                                                 .ToListAsync();

            int cantidadTotal = ganancias.Count;

            return new ListaPaginada<Ganancia> {Lista = ganancias.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Ganancia> ObtenerAsync(string id) => await this.Contexto.Ganancias.FindAsync(id) ?? throw new Exception("Ganancia no encontrada");

        public async Task<Respuesta> CrearAsync(Ganancia modelo)
        {
            if
            (
                await this.Contexto.Ganancias
                          .AnyAsync
                          (e =>
                              (e.Fecha.Date == modelo.Fecha.Date) &&
                              (e.ResponsableId == modelo.ResponsableId)
                          )
            )
                throw new Exception("Ganancia ya registrada");

            await this.Contexto.Ganancias.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Ganancia modelo)
        {
            try
            {
                if
                (
                    await this.Contexto.Ganancias
                              .AnyAsync(e => (e.Fecha.Date == modelo.Fecha.Date) && (e.Id != modelo.Id))
                )
                    throw new Exception("Ganancia ya registrada");

                Ganancia ganancia = await this.Contexto.Ganancias
                                              .Include(e => e.Responsable)
                                              .FirstOrDefaultAsync(e => e.Id == modelo.Id);

                if (ganancia is null) throw new Exception("Ganancia no encontrada");

                ganancia.Fecha = modelo.Fecha;
                ganancia.Cantidad = modelo.Cantidad;
                ganancia.ResponsableId = modelo.ResponsableId;

                this.Contexto.Ganancias.Update(ganancia);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Ganancias.AllAsync(r => r.Id != modelo.Id)) throw new Exception("La ganancia ya no existe");
                throw new Exception("La ganancia fue modificada por alguien más mientras usted trataba de editarla");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Ganancia ganancia = await this.Contexto.Ganancias.FindAsync(id);

                if (ganancia is null) throw new Exception("Ganancia no encontrada");

                this.Contexto.Ganancias.Remove(ganancia);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Ganancias.AllAsync(r => r.Id != id)) throw new Exception("La ganancia ya no existe");
                throw new Exception("La ganancia fue modificada por alguien más mientras usted trataba de borrarla");
            }
        }
    }
}
