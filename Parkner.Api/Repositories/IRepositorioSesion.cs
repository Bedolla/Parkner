using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Parkner.Api.Models;
using Parkner.Core.Extensions;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parkner.Api.Repositories
{
    public interface IRepositorioSesion
    {
        Task<object> AutenticarAsync(Sesion modelo);
    }

    public class RepositorioSesion : IRepositorioSesion
    {
        public RepositorioSesion
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

        public async Task<object> AutenticarAsync(Sesion modelo)
        {
            modelo.Clave = modelo.Clave.Encriptar();

            Cliente cliente = await this.Contexto.Clientes.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave));
            Responsable responsable = await this.Contexto.Responsables.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave));
            Empleado empleado = await this.Contexto.Empleados.FirstOrDefaultAsync(u => u.Correo.Equals(modelo.Correo) && u.Clave.Equals(modelo.Clave));

            if (cliente is not null)
            {
                cliente.Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
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
                            new(ClaimTypes.NameIdentifier, cliente.Id),
                            new(ClaimTypes.Role, cliente.Rol),
                            new(ClaimTypes.Name, cliente.Correo),
                            new(JwtRegisteredClaimNames.Email, cliente.Correo),
                            new("nombre", cliente.Nombre),
                            new("apellido", cliente.Apellido)
                        },
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddHours(24)
                    )
                ));

                return cliente;
            }

            if (responsable is not null)
            {
                responsable.Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
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
                            new(ClaimTypes.NameIdentifier, responsable.Id),
                            new(ClaimTypes.Role, responsable.Rol),
                            new(ClaimTypes.Name, responsable.Correo),
                            new(JwtRegisteredClaimNames.Email, responsable.Correo),
                            new("nombre", responsable.Nombre),
                            new("apellido", responsable.Apellido)
                        },
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddHours(24)
                    )
                ));

                return responsable;
            }

            if (empleado is not null)
            {
                empleado.Token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken
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
                            new(ClaimTypes.NameIdentifier, empleado.Id),
                            new(ClaimTypes.Role, empleado.Rol),
                            new(ClaimTypes.Name, empleado.Correo),
                            new(JwtRegisteredClaimNames.Email, empleado.Correo),
                            new("nombre", empleado.Nombre),
                            new("apellido", empleado.Apellido)
                        },
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddHours(24)
                    )
                ));

                return empleado;
            }

            throw new Exception("Credenciales inválidas");
        }
    }
}
