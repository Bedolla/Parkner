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
    public interface IRepositorioHorarios
    {
        Task<ListaPaginada<Horario>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<Horario> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Horario modelo);
        Task<Respuesta> EditarAsync(Horario modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioHorarios : IRepositorioHorarios
    {
        public RepositorioHorarios
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

        public async Task<ListaPaginada<Horario>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Horario> horarios = await this.Contexto.Horarios
                                               .Buscar(modelo.TerminoBuscado)
                                               .Ordenar(modelo.OrdenarPor)
                                               .ToListAsync();

            int cantidadTotal = horarios.Count;

            return new ListaPaginada<Horario> {Lista = horarios.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Horario> ObtenerAsync(string id) => await this.Contexto.Horarios.FindAsync(id) ?? throw new Exception("Horario no encontrado");

        public async Task<Respuesta> CrearAsync(Horario modelo)
        {
            if
            (
                await this.Contexto.Horarios
                          .AnyAsync
                          (c =>
                              (c.Dia == modelo.Dia) &&
                              (c.Inicio == modelo.Inicio) &&
                              (c.Fin == modelo.Fin) &&
                              (c.EstacionamientoId == modelo.EstacionamientoId)
                          )
            )
                throw new Exception("Horario ya registrado");

            await this.Contexto.Horarios.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Horario modelo)
        {
            try
            {
                Horario horario = await this.Contexto.Horarios.FindAsync(modelo.Id);

                if (horario is null) throw new Exception("Horario no encontrado");

                horario.Dia = modelo.Dia;
                horario.DiaNumero = modelo.DiaNumero;
                horario.Inicio = modelo.Inicio;
                horario.Fin = modelo.Fin;

                if
                (
                    await this.Contexto.Horarios
                              .AnyAsync
                              (c =>
                                  (c.Dia == horario.Dia) &&
                                  (c.Inicio == horario.Inicio) &&
                                  (c.Fin == horario.Fin) &&
                                  (c.EstacionamientoId == horario.EstacionamientoId) &&
                                  (c.Id != horario.Id)
                              )
                )
                    throw new Exception("Horario ya registrado");

                this.Contexto.Horarios.Update(horario);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Horarios.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El horario ya no existe");
                throw new Exception("El horario fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Horario horario = await this.Contexto.Horarios.FindAsync(id);

                if (horario is null) throw new Exception("Horario no encontrado");

                this.Contexto.Horarios.Remove(horario);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Horarios.AllAsync(r => r.Id != id)) throw new Exception("El horario ya no existe");
                throw new Exception("El horario fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }
    }
}
