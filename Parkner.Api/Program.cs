using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Parkner.Api.Migrations;
using System.Globalization;

namespace Parkner.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo cultura = CultureInfo.CreateSpecificCulture("es-MX");
            CultureInfo.DefaultThreadCurrentUICulture = cultura;
            CultureInfo.DefaultThreadCurrentCulture = cultura;

            Program.CreateHostBuilder(args).Build().MigrarBaseDatos().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddConsole();
                    l.AddTraceSource("Error, ActivityTracing");
                })
                .ConfigureWebHostDefaults(w => w.UseStartup<Startup>());
    }
}
