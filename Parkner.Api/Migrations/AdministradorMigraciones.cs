using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parkner.Api.Models;

namespace Parkner.Api.Migrations
{
    public static class AdministradorMigraciones
    {
        public static IHost MigrarBaseDatos(this IHost host)
        {
            using IServiceScope alcance = host.Services.CreateScope();
            using Contexto contexto = alcance.ServiceProvider.GetRequiredService<Contexto>();
            contexto.Database.Migrate();

            return host;
        }
    }
}
