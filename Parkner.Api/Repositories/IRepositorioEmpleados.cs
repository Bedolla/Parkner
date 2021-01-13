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
    public interface IRepositorioEmpleados
    {
        Task<Empleado> ValidarCredencialesAsync(Empleado modelo);
        Task<ListaPaginada<Empleado>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Empleado>> DeAsync(PaginacionPeticion modelo);
        Task<List<Empleado>> NoDeAsync(string id);
        Task<Empleado> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Empleado modelo);
        Task<Respuesta> AgregarAsync(Empleado modelo);
        Task<Respuesta> EditarAsync(Empleado modelo);
        Task<Respuesta> BorrarAsync(string id);
        Task<Respuesta> QuitarAsync(Empleado modelo);
    }

    internal class RepositorioEmpleados : IRepositorioEmpleados
    {
        public RepositorioEmpleados
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

        public async Task<Empleado> ValidarCredencialesAsync(Empleado modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            return await this.Contexto.Empleados.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave)) ?? throw new Exception("Credenciales no validas");
        }

        public async Task<ListaPaginada<Empleado>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Empleado> empleados = await this.Contexto.Empleados
                                                 .Buscar(modelo.TerminoBuscado)
                                                 .Ordenar(modelo.OrdenarPor)
                                                 .ToListAsync();

            int cantidadTotal = empleados.Count;

            return new ListaPaginada<Empleado> {Lista = empleados.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<ListaPaginada<Empleado>> DeAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Empleado> empleados = await this.Contexto.Empleados
                                                 .Include(e => e.Estacionamientos)
                                                 .Where(e => e.Estacionamientos.Any(s => s.Id == modelo.Id))
                                                 .Buscar(modelo.TerminoBuscado)
                                                 .Ordenar(modelo.OrdenarPor)
                                                 .ToListAsync();

            int cantidadTotal = empleados.Count;

            return new ListaPaginada<Empleado> {Lista = empleados.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<List<Empleado>> NoDeAsync(string id)
        {
            Estacionamiento estacionamiento = await this.Contexto.Estacionamientos.FindAsync(id);

            return await this.Contexto
                             .Empleados
                             .Include(e => e.Estacionamientos)
                             .Where
                             (e => e.Estacionamientos.Any
                                 (s =>
                                     (s.ResponsableId == estacionamiento.ResponsableId) &&
                                     (s.Id != id)
                                 )
                             )
                             .OrderBy(e => e.Apellido)
                             .ThenBy(e => e.Nombre)
                             .ThenBy(e => e.Correo)
                             .ToListAsync();
        }

        public async Task<Empleado> ObtenerAsync(string id) => await this.Contexto.Empleados.FirstOrDefaultAsync(u => (u.Id == id) || (u.Correo == id)) ?? throw new Exception("Empleado no encontrado");

        public async Task<Respuesta> CrearAsync(Empleado modelo)
        {
            if
            (
                await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(modelo.Correo))
            )
                throw new Exception("Correo ya registrado");

            string empleadoId = modelo.Id;
            string estacionamientoId = modelo.Rol;

            modelo.Rol = Roles.Empleado;

            await this.Contexto.Empleados.AddAsync(modelo);

            await this.Contexto.EstacionamientoEmpleados.AddAsync(new EstacionamientoEmpleado
            {
                EmpleadoId = empleadoId,
                EstacionamientoId = estacionamientoId
            });

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> AgregarAsync(Empleado modelo)
        {
            string empleadoId = modelo.Id;
            string estacionamientoId = modelo.Rol;

            await this.Contexto.EstacionamientoEmpleados.AddAsync(new EstacionamientoEmpleado
            {
                EmpleadoId = empleadoId,
                EstacionamientoId = estacionamientoId
            });

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Empleado modelo)
        {
            try
            {
                Empleado empleado = await this.Contexto.Empleados.FindAsync(modelo.Id);

                if (empleado is null) throw new Exception("Empleado no encontrado");

                empleado.Nombre = modelo.Nombre;
                empleado.Apellido = modelo.Apellido;
                empleado.Correo = modelo.Correo;
                empleado.Clave = modelo.Clave;
                empleado.Foto = modelo.Foto;

                if
                (
                    await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(empleado.Correo) && (r.Id != empleado.Id)) ||
                    await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(empleado.Correo) && (e.Id != empleado.Id)) ||
                    await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(empleado.Correo) && (u.Id != empleado.Id)) ||
                    await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(empleado.Correo) && (c.Id != empleado.Id))
                )
                    throw new Exception("Correo ya registrado");

                this.Contexto.Empleados.Update(empleado);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Empleados.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El empleado ya no existe");
                throw new Exception("El empleado fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Empleado empleado = await this.Contexto.Empleados.FindAsync(id);

                if (empleado is null) throw new Exception("Empleado no encontrado");

                this.Contexto.Empleados.Remove(empleado);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Empleados.AllAsync(r => r.Id != id)) throw new Exception("El empleado ya no existe");
                throw new Exception("El empleado fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }

        public async Task<Respuesta> QuitarAsync(Empleado modelo)
        {
            string empleadoId = modelo.Id;
            string estacionamientoId = modelo.Rol;

            this.Contexto.EstacionamientoEmpleados.Remove(new EstacionamientoEmpleado
            {
                EmpleadoId = empleadoId,
                EstacionamientoId = estacionamientoId
            });

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }
    }
}
