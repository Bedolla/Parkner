using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Parkner.Api.Models;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parkner.Api.Repositories
{
    public interface IRepositorioClientes
    {
        Task<Cliente> AutenticarAsync(Cliente modelo);
        Task<Cliente> ValidarCredencialesAsync(Cliente peticion);
        Task<ListaPaginada<Cliente>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<Cliente> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Cliente modelo);
        Task<Respuesta> EditarAsync(Cliente modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    public class RepositorioClientes : IRepositorioClientes
    {
        public RepositorioClientes
        (
            Contexto contexto,
            IConfiguration configuracion
        )
        {
            this.Contexto = contexto;
            this.Configuracion = configuracion;
        }

        private Contexto Contexto { get; }
        private IConfiguration Configuracion { get; }

        public async Task<ListaPaginada<Cliente>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Cliente> clientes = await this.Contexto.Clientes
                                               .Buscar(modelo.TerminoBuscado)
                                               .Ordenar(modelo.OrdenarPor)
                                               .ToListAsync();

            int cantidadTotal = clientes.Count;

            return new ListaPaginada<Cliente> {Lista = clientes.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Cliente> ObtenerAsync(string id) => await this.Contexto.Clientes.FirstOrDefaultAsync(u => (u.Id == id) || (u.Correo == id)) ?? throw new Exception("Cliente no encontrado");

        public async Task<Respuesta> CrearAsync(Cliente modelo)
        {
            if
            (
                await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(modelo.Correo))
            )
                throw new Exception("Correo ya registrado");

            await this.Contexto.Clientes.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Cliente modelo)
        {
            try
            {
                Cliente cliente = await this.Contexto.Clientes.FindAsync(modelo.Id);

                if (cliente is null) throw new Exception("Cliente no encontrado");

                cliente.Nombre = modelo.Nombre;
                cliente.Apellido = modelo.Apellido;
                cliente.Correo = modelo.Correo;
                cliente.Clave = modelo.Clave;
                cliente.Foto = modelo.Foto;

                if
                (
                    await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(cliente.Correo) && (r.Id != cliente.Id)) ||
                    await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(cliente.Correo) && (e.Id != cliente.Id)) ||
                    await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(cliente.Correo) && (u.Id != cliente.Id)) ||
                    await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(cliente.Correo) && (c.Id != cliente.Id))
                )
                    throw new Exception("Correo ya registrado");

                this.Contexto.Clientes.Update(cliente);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Clientes.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El cliente ya no existe");
                throw new Exception("El cliente fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Cliente cliente = await this.Contexto.Clientes.FindAsync(id);

                if (cliente is null) throw new Exception("Cliente no encontrado");

                this.Contexto.Clientes.Remove(cliente);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Clientes.AllAsync(r => r.Id != id)) throw new Exception("El cliente ya no existe");
                throw new Exception("El cliente fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }

        public async Task<Cliente> ValidarCredencialesAsync(Cliente peticion)
        {
            peticion.Clave = peticion.Clave.Encriptar();

            return await this.Contexto.Clientes.FirstOrDefaultAsync(u => u.Correo.Equals(peticion.Correo) && u.Clave.Equals(peticion.Clave)) ?? throw new Exception("Credenciales no validas");
        }

        public async Task<Cliente> AutenticarAsync(Cliente modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            Cliente cliente = await this.Contexto.Clientes.FirstOrDefaultAsync(c => c.Correo.Equals(modelo.Correo) && c.Clave.Equals(modelo.Clave));

            if (cliente is null) throw new Exception("Credenciales inválidas");

            cliente.Token = new JwtSecurityTokenHandler().WriteToken
            (
                new JwtSecurityToken
                (
                    new JwtHeader
                    (
                        new SigningCredentials
                        (
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuracion["JWT:ClaveSecreta"])),
                            SecurityAlgorithms.HmacSha256
                        )
                    ),
                    new JwtPayload
                    (
                        this.Configuracion["JWT:Issuer"],
                        this.Configuracion["JWT:Audience"],
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, cliente.Id),
                            new Claim(ClaimTypes.Role, cliente.Rol),
                            new Claim(ClaimTypes.Name, cliente.Correo),
                            new Claim(JwtRegisteredClaimNames.Email, cliente.Correo),
                            new Claim("nombre", cliente.Nombre),
                            new Claim("apellido", cliente.Apellido)
                        },
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddHours(24)
                    )
                )
            );

            return cliente;
        }
    }
}
