using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Parkner.Data.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parkner.Web.Services
{
    public class ProveedorDeEstadoDeAutenticacion : AuthenticationStateProvider
    {
        public ProveedorDeEstadoDeAutenticacion(ISessionStorageService servicioAlmacenamientoSesion) => this.ServicioAlmacenamientoSesion = servicioAlmacenamientoSesion;

        private ISessionStorageService ServicioAlmacenamientoSesion { get; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() =>
            await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Correo") is null ? new ClaimsIdentity() : new ClaimsIdentity(new[]
            {
                new Claim("Nombre", await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Nombre")),
                new Claim("Apellido", await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Apellido")),
                new Claim(ClaimTypes.NameIdentifier, await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Id")),
                new Claim(ClaimTypes.Name, await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Correo")),
                new Claim(ClaimTypes.Role, await this.ServicioAlmacenamientoSesion.GetItemAsync<string>("Rol"))
            }, "apiauth_type"))));

        public void IniciarSesion(Usuario usuario) =>
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("Nombre", usuario.Nombre),
                new Claim("Apellido", usuario.Apellido),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Name, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol)
            }, "apiauth_type")))));

        public async Task CerrarSesion()
        {
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Id");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Nombre");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Apellido");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Correo");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Rol");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Foto");
            await this.ServicioAlmacenamientoSesion.RemoveItemAsync("Token");

            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
