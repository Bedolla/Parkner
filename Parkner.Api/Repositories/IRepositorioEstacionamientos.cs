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
    public interface IRepositorioEstacionamientos
    {
        Task<ListaPaginada<Estacionamiento>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Estacionamiento>> DeAsync(PaginacionPeticion modelo);
        Task<Estacionamiento> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Estacionamiento modelo);
        Task<Respuesta> EditarAsync(Estacionamiento modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioEstacionamientos : IRepositorioEstacionamientos
    {
        public RepositorioEstacionamientos
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

        public async Task<ListaPaginada<Estacionamiento>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Estacionamiento> estacionamientos = await this.Contexto.Estacionamientos
                                                               .Include(e => e.Direccion)
                                                               .Include(e => e.Responsable)
                                                               .Include(e => e.Cajones)
                                                               .Include(e => e.Horarios)
                                                               .Include(e => e.Empleados)
                                                               .Include(e => e.Reservas)
                                                               .Buscar(modelo.TerminoBuscado)
                                                               .Ordenar(modelo.OrdenarPor)
                                                               .ToListAsync();

            int cantidadTotal = estacionamientos.Count;

            return new ListaPaginada<Estacionamiento> {Lista = estacionamientos.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<ListaPaginada<Estacionamiento>> DeAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Estacionamiento> estacionamientos = await this.Contexto.Estacionamientos
                                                               .Include(e => e.Direccion)
                                                               .Include(e => e.Responsable)
                                                               .Include(e => e.Cajones)
                                                               .Include(e => e.Horarios)
                                                               .Include(e => e.Empleados)
                                                               .Include(e => e.Reservas)
                                                               .Where(e => e.ResponsableId == modelo.Id)
                                                               .Buscar(modelo.TerminoBuscado)
                                                               .Ordenar(modelo.OrdenarPor)
                                                               .ToListAsync();

            int cantidadTotal = estacionamientos.Count;

            return new ListaPaginada<Estacionamiento>
            {
                Lista = estacionamientos.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), 
                MetaData = new PaginacionMetaData 
                {
                    CantidadTotal = cantidadTotal, 
                    TamanoPagina = modelo.CantidadPorPagina, 
                    PaginaActual = modelo.NumeroPagina, 
                    PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)

                }
            };
        }

        public async Task<Estacionamiento> ObtenerAsync(string id)
        {
            return await this.Contexto.Estacionamientos
                             .Include(e => e.Direccion)
                             .Include(e => e.Horarios)
                             .Include(e => e.Cajones)
                             .Include(e => e.Reservas)
                             .Include(e => e.Empleados)
                             .SingleOrDefaultAsync(e => e.Id == id) ?? throw new Exception("Estacionamiento no encontrado");
        }

        public async Task<Respuesta> CrearAsync(Estacionamiento modelo)
        {
            if
            (
                await this.Contexto.Estacionamientos
                          .Include(e => e.Direccion)
                          .AnyAsync
                          (e =>
                              e.Direccion.Numero.Equals(modelo.Direccion.Numero) &&
                              e.Direccion.Calle.Equals(modelo.Direccion.Calle) &&
                              e.Direccion.Colonia.Equals(modelo.Direccion.Colonia) &&
                              e.Direccion.CodigoPostal.Equals(modelo.Direccion.CodigoPostal)
                          )
            )
                throw new Exception("Dirección ya registrada");

            await this.Contexto.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Estacionamiento modelo)
        {
            try
            {
                Estacionamiento estacionamiento = await this.Contexto.Estacionamientos
                                                            .Include(e => e.Direccion)
                                                            .SingleOrDefaultAsync(e => e.Id == modelo.Id);

                if (estacionamiento is null) throw new Exception("Estacionamiento no encontrado");

                if (modelo.Nombre != null) estacionamiento.Nombre = modelo.Nombre;
                if (modelo.Descripcion != null) estacionamiento.Descripcion = modelo.Descripcion;
                if (modelo.Costo != default) estacionamiento.Costo = modelo.Costo;
                if (modelo.Foto != null) estacionamiento.Foto = modelo.Foto;
                if (modelo.Calificacion != null) estacionamiento.Calificacion = modelo.Calificacion;
                if (modelo.Tipo != null) estacionamiento.Tipo = modelo.Tipo;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.Numero = modelo.Direccion.Numero;
                if (estacionamiento.Direccion?.Calle != null) estacionamiento.Direccion.Calle = modelo.Direccion.Calle;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.EntreCalles = modelo.Direccion.EntreCalles;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.Colonia = modelo.Direccion.Colonia;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.CodigoPostal = modelo.Direccion.CodigoPostal;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.Municipio = modelo.Direccion.Municipio;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.Latitud = modelo.Direccion.Latitud;
                if (estacionamiento.Direccion?.Numero != null) estacionamiento.Direccion.Longitud = modelo.Direccion.Longitud;

                if
                (
                    await this.Contexto.Estacionamientos
                              .Include(e => e.Direccion)
                              .AnyAsync
                              (e =>
                                  e.Direccion.Numero.Equals(modelo.Direccion.Numero) &&
                                  e.Direccion.Calle.Equals(modelo.Direccion.Calle) &&
                                  e.Direccion.Colonia.Equals(modelo.Direccion.Colonia) &&
                                  e.Direccion.CodigoPostal.Equals(modelo.Direccion.CodigoPostal)
                                  && (e.Direccion.Id != modelo.Direccion.Id)
                              )
                )
                    throw new Exception("Dirección ya registrada");

                this.Contexto.Estacionamientos.Update(estacionamiento);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Estacionamientos.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El estacionamiento ya no existe");
                throw new Exception("El estacionamiento fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Estacionamiento estacionamiento = await this.Contexto.Estacionamientos.FindAsync(id);

                if (estacionamiento is null) throw new Exception("Estacionamiento no encontrado");

                this.Contexto.Estacionamientos.Remove(estacionamiento);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Estacionamientos.AllAsync(r => r.Id != id)) throw new Exception("El estacionamiento ya no existe");
                throw new Exception("El estacionamiento fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }
    }
}
