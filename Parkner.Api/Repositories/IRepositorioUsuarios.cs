using AutoMapper;
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
    public interface IRepositorioUsuarios
    {
        Task<Usuario> ValidarCredencialesAsync(Usuario modelo);
        Task<Usuario> AutenticarAsync(Usuario modelo);
        Task<ListaPaginada<Usuario>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<Usuario> ObtenerAsync(string id);
        Task<Respuesta> CrearAsync(Usuario modelo);
        Task<Respuesta> EditarAsync(Usuario modelo);
        Task<Respuesta> BorrarAsync(string id);
    }

    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        public RepositorioUsuarios
        (
            Contexto contexto,
            IMapper mapeador,
            IConfiguration configuracion
        )
        {
            this.Contexto = contexto;
            this.Mapeador = mapeador;
            this.Configuracion = configuracion;
        }

        private Contexto Contexto { get; }
        private IMapper Mapeador { get; }
        private IConfiguration Configuracion { get; }

        public async Task<Usuario> ValidarCredencialesAsync(Usuario modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            Usuario usuario = await this.Contexto.Usuarios.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave));

            if (usuario is null) throw new Exception("Credenciales inválidas");

            usuario.Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
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
                        new(ClaimTypes.NameIdentifier, usuario.Id),
                        new(ClaimTypes.Role, usuario.Rol),
                        new(ClaimTypes.Name, usuario.Correo),
                        new(JwtRegisteredClaimNames.Email, usuario.Correo),
                        new("nombre", usuario.Nombre),
                        new("apellido", usuario.Apellido)
                    },
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(24)
                )
            ));

            return usuario;
        }

        public async Task<Usuario> AutenticarAsync(Usuario modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            Usuario usuario = await this.Contexto.Usuarios.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave));

            if (usuario is null) throw new Exception("Credenciales inválidas");

            usuario.Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
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
                        new(ClaimTypes.NameIdentifier, usuario.Id),
                        new(ClaimTypes.Role, usuario.Rol),
                        new(ClaimTypes.Name, usuario.Correo),
                        new(JwtRegisteredClaimNames.Email, usuario.Correo),
                        new("nombre", usuario.Nombre),
                        new("apellido", usuario.Apellido)
                    },
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(24)
                )
            ));

            return usuario;
        }

        public async Task<ListaPaginada<Usuario>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            modelo ??= new PaginacionPeticion();

            List<Usuario> usuarios = await this.Contexto.Usuarios
                                               .Where(u => u.Correo != "a@p.co")
                                               .Buscar(modelo.TerminoBuscado)
                                               .Ordenar(modelo.OrdenarPor)
                                               .ToListAsync();

            int cantidadTotal = usuarios.Count;

            return new ListaPaginada<Usuario> {Lista = usuarios.Skip((modelo.NumeroPagina - 1) * modelo.CantidadPorPagina).Take(modelo.CantidadPorPagina).ToList(), MetaData = new PaginacionMetaData {CantidadTotal = cantidadTotal, TamanoPagina = modelo.CantidadPorPagina, PaginaActual = modelo.NumeroPagina, PaginasTotales = (int)Math.Ceiling(cantidadTotal / (double)modelo.CantidadPorPagina)}};
        }

        public async Task<Usuario> ObtenerAsync(string id) => await this.Contexto.Usuarios.FirstOrDefaultAsync(u => (u.Id == id) || (u.Correo == id)) ?? throw new Exception("Usuario no encontrado");

        public async Task<Respuesta> CrearAsync(Usuario modelo)
        {
            if
            (
                await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(modelo.Correo)) ||
                await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(modelo.Correo))
            )
                throw new Exception("Correo ya registrado");

            await this.Contexto.Usuarios.AddAsync(modelo);

            await this.Contexto.SaveChangesAsync();

            return new Respuesta {Tipo = Tipos.Exito};
        }

        public async Task<Respuesta> EditarAsync(Usuario modelo)
        {
            try
            {
                Usuario usuario = await this.Contexto.Usuarios.FindAsync(modelo.Id);

                if (usuario is null) throw new Exception("Usuario no encontrado");

                usuario.Nombre = modelo.Nombre;
                usuario.Apellido = modelo.Apellido;
                usuario.Correo = modelo.Correo;
                usuario.Clave = modelo.Clave;
                usuario.Foto = modelo.Foto;

                if
                (
                    await this.Contexto.Responsables.AnyAsync(r => r.Correo.Equals(usuario.Correo) && (r.Id != usuario.Id)) ||
                    await this.Contexto.Empleados.AnyAsync(e => e.Correo.Equals(usuario.Correo) && (e.Id != usuario.Id)) ||
                    await this.Contexto.Usuarios.AnyAsync(u => u.Correo.Equals(usuario.Correo) && (u.Id != usuario.Id)) ||
                    await this.Contexto.Clientes.AnyAsync(c => c.Correo.Equals(usuario.Correo) && (c.Id != usuario.Id))
                )
                    throw new Exception("Correo ya registrado");

                this.Contexto.Usuarios.Update(usuario);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Usuarios.AllAsync(r => r.Id != modelo.Id)) throw new Exception("El usuario ya no existe");
                throw new Exception("El usuario fue modificado por alguien más mientras usted trataba de editarlo");
            }
        }

        public async Task<Respuesta> BorrarAsync(string id)
        {
            try
            {
                Usuario usuario = await this.Contexto.Usuarios.FindAsync(id);

                if (usuario is null) throw new Exception("Usuario no encontrado");

                this.Contexto.Usuarios.Remove(usuario);

                await this.Contexto.SaveChangesAsync();

                return new Respuesta {Tipo = Tipos.Exito};
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.Contexto.Usuarios.AllAsync(r => r.Id != id)) throw new Exception("El usuario ya no existe");
                throw new Exception("El usuario fue modificado por alguien más mientras usted trataba de borrarlo");
            }
        }
    }
}
