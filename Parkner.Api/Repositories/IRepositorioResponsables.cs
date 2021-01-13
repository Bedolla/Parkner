using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parkner.Api.Models;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Api.Repositories
{
    public interface IRepositorioResponsables
    {
        Task<Responsable> ValidarCredencialesAsync(Responsable modelo);
        Task<ListaPaginada<Responsable>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<Responsable> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Responsable modelo);
        Task<Respuesta> EditarAsync(Responsable modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    internal class RepositorioResponsables : IRepositorioResponsables
    {
        public RepositorioResponsables
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

        public async Task<Responsable> ValidarCredencialesAsync(Responsable modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            return await this.Contexto.Responsables.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave)) ?? throw new Exception("Credenciales no validas");
        }

        public async Task<ListaPaginada<Responsable>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Responsable> responsables = await this.Contexto.Responsables
                                                       .Buscar(modelo.TerminoBuscado)
                                                       .Ordenar(modelo.OrdenarPor)
                                                       .ToListAsync();

            int cantidadTotal = responsables.Count;

            return new ListaPaginada<Responsable> {Lista = responsables.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Responsable> ObtenerAsync(string id) => await this.Contexto.Responsables.FirstOrDefaultAsync(u => (u.Id == id) || (u.Correo == id)) ?? throw new Exception("Responsable no encontrado");

        public async Task<Respuesta> CrearAsync(Responsable modelo)
        {
            if
            (
                await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(modelo.Correo))
            )
                throw new Exception("Correo ya registrado");

            await this.Contexto.Responsables.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Responsable modelo)
        {
            try
            {
                Responsable responsable = await this.Contexto.Responsables.FindAsync(modelo.Id);

                if (responsable is null) throw new Exception("Responsable no encontrado");

                responsable.Nombre = modelo.Nombre;
                responsable.Apellido = modelo.Apellido;
                responsable.Correo = modelo.Correo;
                responsable.Clave = modelo.Clave;
                responsable.Foto = modelo.Foto;

                if
                (
                    await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(responsable.Correo) && (r.Id != responsable.Id)) ||
                    await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(responsable.Correo) && (e.Id != responsable.Id)) ||
                    await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(responsable.Correo) && (u.Id != responsable.Id)) ||
                    await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(responsable.Correo) && (c.Id != responsable.Id))
                )
                    throw new Exception("Correo ya registrado");

                this.Contexto.Responsables.Update(responsable);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Responsables.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El responsable ya no existe");
                throw new Exception("El responsable fue modificado por alguien más mientras usted trataba de modificarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Responsable responsable = await this.Contexto.Responsables.FindAsync(id);

                if (responsable is null) throw new Exception("Responsable no encontrado");

                this.Contexto.Responsables.Remove(responsable);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Responsables.AllAsync(r => r.Id != id)) throw new Exception("El responsable ya no existe");
                throw new Exception("El responsable fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }
    }
}
